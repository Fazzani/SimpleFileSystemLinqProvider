using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Linq;
using System.Text.RegularExpressions;

namespace LinqFileSystemProvider.Common
{

    internal class ObjectReader<T> : IEnumerable<T>, IEnumerable
    {
        private IEnumerator<T> enumerator;

        internal ObjectReader(IEnumerable<FileSystemElement> readerEnumerator)
        {
            Type outType = typeof(T);
            if (outType == typeof(FileSystemElement))
                this.enumerator = readerEnumerator.GetEnumerator() as IEnumerator<T>;
            else
                this.enumerator = new Enumerator(readerEnumerator.GetEnumerator());
        }

        public IEnumerator<T> GetEnumerator()
        {
            IEnumerator<T> e = this.enumerator;

            if (e == null)
                throw new InvalidOperationException("Cannot enumerate more than once");

            this.enumerator = null;
            return e;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private class Enumerator : IEnumerator<T>, IEnumerator, IDisposable
        {
            readonly Type outType = typeof(T);
            private IEnumerator<FileSystemElement> reader;
            private FieldInfo[] fields;
            private int[] fieldLookup;
            readonly PropertyInfo[] fsFields = typeof(FileSystemElement).GetProperties();

            internal Enumerator(IEnumerator<FileSystemElement> reader)
            {
                this.reader = reader;
                this.fields = typeof(T).GetRuntimeFields().ToArray();
            }

            public T Current { get; private set; }

            object IEnumerator.Current { get { return this.Current; } }

            public bool MoveNext()
            {
                if (this.reader.MoveNext())
                {
                    if (this.fieldLookup == null)
                        this.InitFieldLookup();

                    T instance = default(T);
                    if (TypeSystem.IsAnonymousType(outType))
                    {
                        instance = (T)FormatterServices.GetUninitializedObject(outType);
                    }
                    else if (!outType.IsAssignableFrom(typeof(string)))
                        instance = Activator.CreateInstance<T>();
                    //if (instance == null)
                    //    instance = reader.Current.ToString();
                    //else
                        for (int i = 0, n = this.fields.Length; i < n; i++)
                        {
                            int index = this.fieldLookup[i];
                            if (index >= 0)
                            {
                                var fi = this.fields[i];
                                var fsField = fsFields[i];
                                if (fsField == null)
                                    fi.SetValue(instance, null);
                                else
                                    fi.SetValue(instance, fsField.GetValue(reader.Current));
                            }
                        }

                    this.Current = instance;
                    return true;
                }
                return false;
            }

            public void Reset() => reader.Reset();

            public void Dispose() => reader.Dispose();

            private void InitFieldLookup()
            {
                var map = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);

                for (int i = 0, n = this.fsFields.Length; i < n; i++)
                    map.Add(fsFields[i].Name, i);

                this.fieldLookup = new int[this.fields.Length];

                for (int i = 0, n = this.fields.Length; i < n; i++)
                {
                    int index;
                    string result = new Regex("<(.+)>(.*)$").Replace(this.fields[i].Name, "$1");

                    if (map.TryGetValue(result, out index))
                        this.fieldLookup[i] = index;
                    else
                        this.fieldLookup[i] = -1;
                }
            }
        }

    }
}