#region

using System;
using UnityEngine;

#endregion

namespace CodeBase.Logic
{
    public class PlayerTriggerReporter : MonoBehaviour
    {
        private const string PlayerTag = "Player";
        public Action OnTriggeredEnter;
        public Action OnTriggeredStay;
        public Action OnTriggeredExit;
        public Collider2D Collider { get; private set; }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(PlayerTag))
            {
                Collider = other;
                OnTriggeredEnter?.Invoke();
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag(PlayerTag))
            {
                Collider = other;
                OnTriggeredStay?.Invoke();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(PlayerTag))
            {
                Collider = other;
                OnTriggeredExit?.Invoke();
            }
        }
    }
}