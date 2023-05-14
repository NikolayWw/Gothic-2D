#region

using CodeBase.Logic.Move;
using UnityEngine;
using UnityEngine.AI;

#endregion

namespace CodeBase.Npc
{
    public class NpcMover : MonoBehaviour, ILookDirection
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private NpcHealth _npcHealth;
        public Vector2 LookDirection { get; private set; }
        private Transform _target;

        private float _xScale;

        private void Start()
        {
            _xScale = transform.localScale.x;
            _npcHealth.Happened += Pause;
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
        }

        private void Update()
        {
            var directionToTarget = DirectionToTarget();
            UpdateFlip(directionToTarget);
            UpdateLookDirection(directionToTarget);
        }

        public void SetSpeed(float speed) => _agent.speed = speed;

        public void SetTarget(Transform target)
        {
            _target = target;
            _agent.SetDestination(_target.position);
            _agent.stoppingDistance = 0;
        }

        public void SetStoppingDistance(float value)
        {
            _agent.stoppingDistance = value;
        }

        public void ClearVelocity()
        {
            _agent.velocity *= 0;
        }

        public bool IsMoving()
        {
            return _agent.velocity.magnitude > 0.2f; //min speed;
        }

        private void UpdateLookDirection(Vector2 targetDirection)
        {
            if (Mathf.Abs(targetDirection.y) > Mathf.Abs(targetDirection.x))
                LookDirection =
                    targetDirection.y > 0 ? Vector2.up :
                    targetDirection.y < 0 ? Vector2.down :
                    LookDirection;
            else
                LookDirection =
                    targetDirection.x > 0 ? Vector2.right :
                    targetDirection.x < 0 ? Vector2.left :
                    LookDirection;
        }

        private void UpdateFlip(Vector2 targetDirection)
        {
            var scale = transform.localScale;
            scale.x =
                targetDirection.x < 0 ? _xScale :
                targetDirection.x > 0 ? -_xScale :
                scale.x;
            transform.localScale = scale;
        }

        private Vector2 DirectionToTarget()
        {
            if ((_target != null && _target != transform) == false)
                return transform.position;

            Vector2 direction = _target.position - transform.position;
            return direction;
        }

        private void Pause()
        {
            SetTarget(transform);
            _agent.velocity *= 0;
        }
    }
}