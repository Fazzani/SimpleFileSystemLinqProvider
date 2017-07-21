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
        public LambdaExpression WhereExpression { get; private set; }
        public LambdaExpression SelectExpression { get; private set; }

        internal ExpressionTreeModifier(IQueryable<FileSystemElement> elements)
        {
            //System.Console.WriteLine("ExpressionTreeModifier()");
            this.fileSystemElements = elements;
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {

            if (m.Method.DeclaringType == typeof(Queryable))
            {
                switch (m.Method.Name)
                {
                    case "Select":
                        Visit(m.Arguments[0]);
                        SelectExpression = (LambdaExpression)StripQuotes(m.Arguments[1]);
                        //Visit(SelectExpression.Body);
                        return m;
                    case "OrderBy":
                        Visit(m.Arguments[0]);
                        return m;
                    case "Where":
                        WhereExpression = (LambdaExpression)StripQuotes(m.Arguments[1]);
                        Visit(WhereExpression.Body);
                        return m;
                }

                throw new NotSupportedException("");

            }
            return m;
        }

        public static Expression StripQuotes(Expression e)
        {
            while (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression)e).Operand;
            }
            return e;
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