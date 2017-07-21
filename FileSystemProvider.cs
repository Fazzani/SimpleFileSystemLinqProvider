using LinqFileSystemProvider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LinqFileSystemProvider
{
    public class FileSystemProvider : QueryProvider
    {
        private readonly string root;

        public FileSystemProvider(string root)
        {
            //System.Console.WriteLine("FileSystemProvider()");
            this.root = root;
        }

        //public IQueryable CreateQuery(Expression expression)
        //{
        //    return new FileSystemContext(this, expression);
        //}

        //public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        //{
        //    return (IQueryable<TElement>)new FileSystemContext(this, expression);
        //}

        //public object Execute(Expression expression)
        //{
        //    return Execute<FileSystemElement>(expression);
        //}

        //public TResult Execute<TResult>(Expression expression)
        //{
        //    var isEnumerable = (typeof(TResult).Name == "IEnumerable`1");
        //    return (TResult)FileSystemQueryContext.Execute(expression, isEnumerable, root);
        //}

        public override object Execute(Expression expression)
        {
            Type elementType = TypeSystem.GetElementType(expression.Type);
            var isEnumerable = elementType.Name == "IEnumerable`1";

            var reader = FileSystemQueryContext.Execute<FileSystemElement>(expression, isEnumerable, root);
            
            return Activator.CreateInstance(
                typeof(ObjectReader<>).MakeGenericType(elementType),
                BindingFlags.Instance | BindingFlags.NonPublic, null,
                new object[] { reader },
                null);
        }

        public override string GetQueryText(Expression expression)
        {
            //return this.Translate(expression);
            return default(string);
        }
        private string Translate(Expression expression)
        {
            // return new QueryTranslator().Translate(expression);
            return default(string);
        }
    }
}
