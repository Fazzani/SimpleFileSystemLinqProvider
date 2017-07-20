using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LinqFileSystemProvider
{
    public class FileSystemQueryContext
    {
        internal static object Execute(Expression expression, bool isEnumerable, string root)
        {
            //System.Console.WriteLine("FileSystemQueryContext.Execute()");
            var queryableElements = GetAllFilesAndFolders(root);

            // Copy the expression tree that was passed in, changing only the first
            // argument of the innermost MethodCallExpression.
            var treeCopier = new ExpressionTreeModifier(queryableElements);
            var newExpressionTree = treeCopier.Visit (expression);

            // This step creates an IQueryable that executes by replacing Queryable methods with Enumerable methods.
            if (isEnumerable)
            {
                //System.Console.WriteLine("FileSystemQueryContext.Execute(isEnumerable=True)");
                return queryableElements.Provider.CreateQuery(newExpressionTree);
            }
            else
            {
                //System.Console.WriteLine("FileSystemQueryContext.Execute(isEnumerable=False)");
                return queryableElements.Provider.Execute(newExpressionTree);
            }
        }

        public static IQueryable<FileSystemElement> GetAllFilesAndFolders(string root)
        {
            var list = new List<FileSystemElement>();
            foreach (var directory in Directory.GetDirectories(root))
            {
                list.Add(new FolderElement(directory));
            }
            foreach (var file in Directory.GetFiles(root))
            {
                list.Add(new FileElement(file));
            }
            return list.AsQueryable();
        }     
     }
}
