﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LinqFileSystemProvider.Common
{
    public class Query<T> : IQueryable<T>, IQueryable, IEnumerable<T>, IEnumerable, IOrderedQueryable<T>, IOrderedQueryable
    {
        QueryProvider _provider;
        Expression _expression;

        public Query(QueryProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException("provider");
            _expression = Expression.Constant(this);
        }

        public Query(QueryProvider provider, Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            if (!typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
                throw new ArgumentOutOfRangeException("expression");

            _provider = provider ?? throw new ArgumentNullException("provider");
            _expression = expression;
        }

        Expression IQueryable.Expression
        {
            get { return _expression; }
        }

        Type IQueryable.ElementType
        {
            get { return typeof(T); }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return _provider; }
        }

        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_provider.Execute(_expression)).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_provider.Execute(_expression)).GetEnumerator();

        public override string ToString() => _provider.GetQueryText(_expression);
    }
}
