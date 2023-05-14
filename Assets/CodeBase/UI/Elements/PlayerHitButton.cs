using CodeBase.Player;
using CodeBase.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class PlayerHitButton : BaseWindow
    {
        [SerializeField] private Button _hitButton;
        private PlayerKiller _playerKiller;

        public void Construct(PlayerKiller playerKiller)
        {
            _playerKiller = playerKiller;
            _hitButton.onClick.AddListener(Hit);
        }

        private void Hit()
        {
            if (_playerKiller.ConditionToHit())
                _playerKiller.Hit();
        }
    }
}