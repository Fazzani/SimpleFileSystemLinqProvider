﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LinqFileSystemProvider
{
    public class FileSystemContext : IOrderedQueryable<FileSystemElement>
    {
        public FileSystemContext(string root)
        {
            //System.Console.WriteLine("FileSystemContext()");
            Provider = new FileSystemProvider(root);
            Expression = Expression.Constant(this);
        }

        internal FileSystemContext(IQueryProvider provider, Expression expression)
        {
            Provider = provider;
            Expression = expression;
        }
        /// <summary>
        /// Return a type-safe Enumerator.
        /// <remarks>Unfortunately framework wants a non-generic Enumerator.</remarks>
        /// </summary>
        /// <returns>IEnumerator</returns>
        public IEnumerator<FileSystemElement> GetEnumerator()
        {
            //System.Console.WriteLine("GetEnumerator(1)");
            return Provider.Execute<IEnumerable<FileSystemElement>>(Expression).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            //System.Console.WriteLine("GetEnumerator(2)");
            // call the generic version of the method
            return this.GetEnumerator();
        }

        public Type ElementType
        {
            get { return typeof(FileSystemElement); }
        }

        public Expression Expression { get; private set; }
        public IQueryProvider Provider { get; private set; }
    }
}
