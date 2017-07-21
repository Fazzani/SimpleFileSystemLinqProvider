using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LinqFileSystemProvider.Common
{
    internal class ColumnProjection
    {
        internal string Columns;
        internal Expression Selector;
    }
}
