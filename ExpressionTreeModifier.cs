using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LinqFileSystemProvider
{
    internal class ExpressionTreeModifier : ExpressionVisitor
    {
        private IQueryable<FileSystemElement> fileSystemElements;

        internal ExpressionTreeModifier(IQueryable<FileSystemElement> elements)
        {
            //System.Console.WriteLine("ExpressionTreeModifier()");
            this.fileSystemElements = elements;
        }
        
        internal Expression CopyAndModify(Expression expression)
        {
            System.Console.WriteLine("CopyAndModify()");
            return this.Visit(expression);
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            //StringBuilder s = new StringBuilder ();
            //s.AppendFormat("CopyAndModify.VisitConstant expression type {0}", c.Type.ToString());
            //System.Console.WriteLine(s.ToString ());
           // if (c.Type == typeof(IQueryable<LinqFileSystemProvider.FileSystemContext>))
                if (c.Type == typeof(LinqFileSystemProvider.FileSystemContext))
                {
                //System.Console.WriteLine("CopyAndModify.VisitConstant(IQueryable<FileSystemContext)");
                return Expression.Constant(this.fileSystemElements);
            }
            else
            {
                //System.Console.WriteLine("CopyAndModify.VisitConstant(uninteresting type)");
                return c;
            }
        }
    }
}