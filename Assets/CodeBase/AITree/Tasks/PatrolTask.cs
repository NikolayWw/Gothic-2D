using CodeBase.AITree.Tree;
using CodeBase.Npc;
using UnityEngine;

namespace CodeBase.AITree.Tasks
{
    public class PatrolTask : Node
    {
        private readonly NpcMover _npcMover;
        private readonly Transform _transform;
        private readonly Vector2[] _waypoints;
        private readonly Transform _waypoint;
        private readonly float _speed;

        private int _currentIndex;

        public PatrolTask(NpcMover npcMover, Transform transform, Vector2[] waypoints, float speed)
        {
            _speed = speed;
            _npcMover = npcMover;
            _transform = transform;
            _waypoints = waypoints;
            _waypoint = new GameObject("Waypoint").transform;
        }

        public override bool Evaluate()
        {
            UpdateWaypointDistance();
            _npcMover.SetTarget(CurrentPoint());
            _npcMover.SetSpeed(_speed);
            return true;
        }

        private void UpdateWaypointDistance()
        {
            if (Vector2.Distance(CurrentPoint().position, _transform.position) < 0.2f) //min distance
            {
                _currentIndex = (_currentIndex + 1) % _waypoints.Length;
                _npcMover.ClearVelocity();
            }
        }

        private Transform CurrentPoint()
        {
            _waypoint.position = _waypoints[_currentIndex];
            return _waypoint;
        }
    }
}