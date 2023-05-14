using System.Collections.Generic;
using System.Linq;

namespace CodeBase.AITree.Tree
{
    public class Sequencer : Node
    {
        public Sequencer(List<Node> children) : base(children)
        {
        }

        public override bool Evaluate()
        {
            return _children.All(child => child.Evaluate());
        }
    }
}