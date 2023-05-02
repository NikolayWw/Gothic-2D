using CodeBase.AITree.Tree;
using CodeBase.Npc;

namespace CodeBase.AITree.Tasks
{
    public class FollowTask : Node
    {
        private readonly NpcMover _npcMover;
        private readonly float _speed;

        public FollowTask(NpcMover npcMover, float speed)
        {
            _npcMover = npcMover;
            _speed = speed;
        }

        public override bool Evaluate()
        {
            var target = GetTarget();
            if (target == null)
                return false;

            _npcMover.SetSpeed(_speed);
            _npcMover.SetTarget(target);
            return true;
        }
    }
}