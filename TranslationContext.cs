using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LinqFileSystemProvider
{
    internal class TranslationContext<T>
    {
        public TranslationContext(IQueryable<T> source)
        {
            Source = source;
            SelectedMembers = new List<string>();
        }
        public TranslationContext(IQueryable<T> source, Expression<Func<T, bool>> whereExpression) : this(source)
        {
            WhereExpression = whereExpression;
        }

        public TranslationContext(IQueryable<T> source, Expression<Func<T, bool>> whereExpression, List<string> selectedMembers)
            : this(source, whereExpression)
        {
            SelectedMembers = selectedMembers;
        }

        public IQueryable<T> Source { get; private set; }
        public List<string> SelectedMembers { get; private set; }
        public Expression<Func<T, bool>> WhereExpression { get; private set; }
    }
}
