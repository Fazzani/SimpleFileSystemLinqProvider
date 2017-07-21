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
        private IEnumerator<T> _enumerator;

        internal ObjectReader(IEnumerable<FileSystemElement> readerEnumerator)
        {
            Type outType = typeof(T);
            if (outType == typeof(FileSystemElement))
                _enumerator = readerEnumerator.GetEnumerator() as IEnumerator<T>;
            else
                _enumerator = new Enumerator(readerEnumerator.GetEnumerator());
        }

        public IEnumerator<T> GetEnumerator()
        {
            IEnumerator<T> e = _enumerator;

            if (e == null)
                throw new InvalidOperationException("Cannot enumerate more than once");

            _enumerator = null;
            return e;
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        private class Enumerator : IEnumerator<T>, IEnumerator, IDisposable
        {
            readonly Type  _outType = typeof(T);
            private IEnumerator<FileSystemElement> _reader;
            private FieldInfo[] _fields;
            private int[] _fieldLookup;
            readonly PropertyInfo[] _fsFields = typeof(FileSystemElement).GetProperties();

            internal Enumerator(IEnumerator<FileSystemElement> reader)
            {
                _reader = reader;
                _fields = typeof(T).GetRuntimeFields().ToArray();
            }

            public T Current { get; private set; }

            object IEnumerator.Current { get { return this.Current; } }

            public bool MoveNext()
            {
                if (_reader.MoveNext())
                {
                    if (_fieldLookup == null)
                        InitFieldLookup();

                    T instance = TypeSystem.New<T>.Instance();

                    for (int i = 0, n = _fields.Length; i < n; i++)
                    {
                        int index = _fieldLookup[i];
                        if (index >= 0)
                        {
                            var fi = this._fields[i];
                            var fsField = _fsFields[i];
                            if (fsField == null)
                                fi.SetValue(instance, null);
                            else
                                fi.SetValue(instance, fsField.GetValue(_reader.Current));
                        }
                    }

                    this.Current = instance;
                    return true;
                }
                return false;
            }

            public void Reset() => _reader.Reset();

            public void Dispose() => _reader.Dispose();

            private void InitFieldLookup()
            {
                var map = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);

                for (int i = 0, n = _fsFields.Length; i < n; i++)
                    map.Add(_fsFields[i].Name, i);

                _fieldLookup = new int[_fields.Length];

                for (int i = 0, n = _fields.Length; i < n; i++)
                {
                    string result = new Regex("<(.+)>(.*)$").Replace(_fields[i].Name, "$1");

                    if (map.TryGetValue(result, out int index))
                        _fieldLookup[i] = index;
                    else
                        _fieldLookup[i] = -1;
                }
            }
        }

    }
}