using LinqFileSystemProvider.Common;
using System;
using System.Collections.Generic;
using System.IO;
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

        public override object Execute(Expression expression)
        {
            Type elementType = TypeSystem.GetElementType(expression.Type);
            var isEnumerable = elementType.Name == "IEnumerable`1";

            var queryableElements = GetAllFilesAndFolders(root);
            var treeCopier = new ExpressionTreeModifier(queryableElements);
            var selectedMembersVisitor = new SelectedMembersVisitor();
            var newExpressionTree = treeCopier.Visit(expression);
            selectedMembersVisitor.Visit(treeCopier.SelectExpression);
            //TODO: catch the functions : OrderBy, Take, first, Single
            var context = new TranslationContext<FileSystemElement>(queryableElements, treeCopier.WhereExpression as Expression<Func<FileSystemElement, bool>>, selectedMembersVisitor.Members);

            return Activator.CreateInstance(
                typeof(ObjectReader<>).MakeGenericType(elementType),
                BindingFlags.Instance | BindingFlags.NonPublic, null,
                new object[] { context },
                null);
        }

        private static IQueryable<FileSystemElement> GetAllFilesAndFolders(string root)
        {
            var list = new List<FileSystemElement>();
            foreach (var directory in Directory.GetDirectories(root))
                list.Add(new FolderElement(directory));
            foreach (var file in Directory.GetFiles(root))
                list.Add(new FileElement(file));
            return list.AsQueryable();
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
