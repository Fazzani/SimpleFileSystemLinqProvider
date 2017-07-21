using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LinqFileSystemProvider
{
    internal class SelectedMembersVisitor : ExpressionVisitor
    {
        public List<string> Members { get; private set; }

        public SelectedMembersVisitor()
        {
            Members = new List<string>();
        }
        protected override Expression VisitMember(MemberExpression node)
        {
            var member = node as MemberExpression;
            if (member == null)
                return base.VisitMember(node);
            Members.Add(member.Member.Name);
            return node;
        }
    }
}