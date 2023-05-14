#region

using CodeBase.Logic;
using System.Collections;
using UnityEngine;

#endregion

namespace CodeBase.Npc
{
    public class NpcFollowTarget : MonoBehaviour
    {
        [SerializeField] private PlayerTriggerReporter playerTriggerReporter;
        [SerializeField] private NpcMover _npcMover;
        private Transform _target;
        private IEnumerator _followEnumerator;
        private bool _nearTarget;

        private void Awake()
        {
            playerTriggerReporter.OnTriggeredEnter += Follow;
            _followEnumerator = UpdateTargetPosition();
        }

        private void Follow()
        {
            playerTriggerReporter.OnTriggeredEnter -= Follow;
            _target = playerTriggerReporter.Collider.transform;
            StartCoroutine(_followEnumerator);
        }

        private IEnumerator UpdateTargetPosition()
        {
            while (true)
            {
                if (Vector2.Distance(transform.position, _target.position) > 0.8f)
                {
                    _nearTarget = false;
                    _npcMover.SetTarget(_target);
                }
                else if (_nearTarget == false)
                {
                    _nearTarget = true;
                    _npcMover.SetStoppingDistance(Vector2.Distance(transform.position, _target.position) + 0.8f);
                    _npcMover.ClearVelocity();
                }

                yield return null;
            }
        }
    }
}