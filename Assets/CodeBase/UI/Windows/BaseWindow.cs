using CodeBase.Logic.AnimationsStateReporter;
using CodeBase.UI.Services.Window;
using System;
using UnityEngine;

namespace CodeBase.UI.Windows
{
    public abstract class BaseWindow : MonoBehaviour, IAnimatorStateReader
    {
        [SerializeField] private WindowAnimation _windowAnimation;

        public Action OnClosed;
        public Action<WindowId> OnClosedSendId;
        public WindowId Id { get; private set; }

        public void SetId(WindowId id) =>
            Id = id;

        private void Awake()
        {
            if (_windowAnimation.UseAnimation)
                _windowAnimation.PlayOpen();
        }

        public void Close()
        {
            OnClosedSendId?.Invoke(Id);
            OnClosed?.Invoke();

            OnClosedSendId = null;
            OnClosed = null;
            OnClose();

            if (_windowAnimation.UseAnimation)
                _windowAnimation.PlayClose(DestroyThis);
            else
                DestroyThis();
        }

        public void ExitedState(int shortNameHash)
        {
            _windowAnimation.ExitedState(shortNameHash);
        }

        protected virtual void OnClose()
        { }

        private void DestroyThis() =>
            Destroy(gameObject);
    }
}