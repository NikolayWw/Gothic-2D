#region

using CodeBase.Logic;
using UnityEngine;

#endregion

namespace CodeBase.Npc
{
    public class FindTarget : MonoBehaviour
    {
        [SerializeField] private PlayerTriggerReporter playerTriggerReporter;
        [SerializeField] private NpcBehaviour _npcBehaviour;

        private void Awake()
        {
            playerTriggerReporter.OnTriggeredEnter += () => _npcBehaviour.SetTarget(playerTriggerReporter.Collider.transform);
        }
    }
}