using CodeBase.Logic.AnimationsStateReporter;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    [Serializable]
    public class WindowAnimation : IAnimatorStateReader
    {
        private readonly int OpenHash = Animator.StringToHash("Open");
        private readonly int CloseHash = Animator.StringToHash("Close");

        [field: SerializeField] public bool UseAnimation { get; private set; }
        [field: SerializeField] public GraphicRaycaster _graphicRaycaster { get; private set; }
        [field: SerializeField] public Animator _animator { get; private set; }
        private Action _onClosed;

        public void ExitedState(int shortNameHash)
        {
            if (shortNameHash == CloseHash)
                _onClosed?.Invoke();
        }

        public void PlayClose(Action onClosed = null)
        {
            _onClosed = onClosed;
            _animator.Play(CloseHash);
            DisableRaycast();
        }

        public void PlayOpen() =>
            _animator.Play(OpenHash);

        public bool IsUseAnimationAndPresent() =>
            UseAnimation && _animator != null;

        private void DisableRaycast()
        {
            if (_graphicRaycaster)
                _graphicRaycaster.enabled = false;
        }
    }
}