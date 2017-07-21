using LinqFileSystemProvider.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LinqFileSystemProvider
{
    public class FileSystemContext : Query<FileSystemElement>
    {
        public FileSystemContext(string root) : base(new FileSystemProvider(root))
        {
        }

        internal FileSystemContext(QueryProvider provider, Expression expression) : base(provider, expression)
        {
        }
        
    }
}
