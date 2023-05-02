using CodeBase.AITree.Tree;
using CodeBase.Npc;

namespace CodeBase.AITree.Tasks
{
    public class ApplyTargetTask : Node
    {
        private readonly NpcBehaviour _npcBehaviour;

        public ApplyTargetTask(NpcBehaviour npcBehaviour)
        {
            _npcBehaviour = npcBehaviour;
        }

        public override bool Evaluate()
        {
            if (_npcBehaviour.Target)
            {
                Parent.Parent.SetTarget(_npcBehaviour.Target);
                return true;
            }

            return false;
        }
    }
}