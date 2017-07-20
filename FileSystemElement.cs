using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LinqFileSystemProvider
{
    public enum ElementType
    {
        File,
        Folder
    }

    public abstract class FileSystemElement
    {
        public string Path { get; private set; }
        public abstract ElementType ElementType { get; }

        protected FileSystemElement(string path)
        {
            Path = path;
        }

        public abstract new string ToString();
    }

    public class FolderElement : FileSystemElement
    {
        public FolderElement(string path)
            : base(path)
        {
        }
        public override ElementType ElementType { get { return ElementType.Folder; } }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            s.AppendFormat("Folder {0}", Path);
            return s.ToString();
        }
    }

    public class FileElement : FileSystemElement
    {
        public FileElement(string path)
            : base(path)
        {
        }
        public override ElementType ElementType { get { return ElementType.File; } }
        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            s.AppendFormat("File {0}", Path);
            return s.ToString();
         }
    }

}
