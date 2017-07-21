using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LinqFileSystemProvider.Common;

namespace LinqFileSystemProvider
{

    /// <summary>
    /// <see cref="http://www.la-solutions.co.uk/content/DotNet/DotNet-SimpleLinqDataProvider.htm#Download"/>
    /// </summary>
    public class FileSystemQueryContext
    {
        internal static object Execute<T>(Expression expression, bool isEnumerable, string root)
        {
            var queryableElements = GetAllFilesAndFolders(root);
            var treeCopier = new ExpressionTreeModifier(queryableElements);
            var newExpressionTree = treeCopier.Visit(expression);
            if (treeCopier.WhereExpression == null)
                return queryableElements;
            return queryableElements.Where(treeCopier.WhereExpression as Expression<Func<FileSystemElement, bool>>);
        }

        public static IQueryable<FileSystemElement> GetAllFilesAndFolders(string root)
        {
            var list = new List<FileSystemElement>();
            foreach (var directory in Directory.GetDirectories(root))
                list.Add(new FolderElement(directory));
            foreach (var file in Directory.GetFiles(root))
                list.Add(new FileElement(file));
            return list.AsQueryable();
        }
    }
}
