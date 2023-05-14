using CodeBase.AITree.Tree;
using CodeBase.Npc;
using UnityEngine;

namespace CodeBase.AITree.Tasks
{
    public class CheckAttackDistanceTask : Node
    {
        private const float MinDistance = 0.8f;

        private readonly NpcMover _npcMover;
        private readonly Transform _transform;

        public CheckAttackDistanceTask(NpcMover npcMover, Transform transform)
        {
            _npcMover = npcMover;
            _transform = transform;
        }

        public override bool Evaluate()
        {
            var target = GetTarget();
            if (target == null)
                return false;

            var distance = Vector2.Distance(target.position, _transform.position);
            if (distance < MinDistance)
            {
                _npcMover.SetStoppingDistance(distance + MinDistance);
                _npcMover.ClearVelocity();
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}