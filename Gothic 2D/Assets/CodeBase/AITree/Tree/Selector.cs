using System.Collections.Generic;
using System.Linq;

namespace CodeBase.AITree.Tree
{
    public class Selector : Node
    {
        public Selector(List<Node> children) : base(children)
        { }

        public override bool Evaluate()
        {
            return _children.Any(child => child.Evaluate());
        }
    }
}