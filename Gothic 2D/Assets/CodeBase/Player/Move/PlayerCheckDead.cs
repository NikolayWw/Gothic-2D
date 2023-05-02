#region

using CodeBase.Data.PlayerProgress.Player;
using CodeBase.Logic.Move;
using CodeBase.Services.PersistentProgress;
using System;
using UnityEngine;

#endregion

namespace CodeBase.Player.Move
{
    public class PlayerCheckDead : MonoBehaviour
    {
        [SerializeField] private MoverAnimation _animation;
        public Action Happened;
        public bool IsDead { get; private set; }
        private PlayerCharacteristics _playerCharacteristics;

        public void Construct(IPersistentProgressService persistentProgressService)
        {
            _playerCharacteristics = persistentProgressService.PlayerProgress.PlayerData.PlayerCharacteristics;
            _playerCharacteristics.OnChangeCurrentHealth += CheckDead;
        }

        private void OnDestroy()
        {
            _playerCharacteristics.OnChangeCurrentHealth -= CheckDead;
            Happened = null;
        }

        private void CheckDead()
        {
            if (_playerCharacteristics.CurrentHealth <= 0)
            {
                IsDead = true;
                Happened?.Invoke();
                _animation.PlayDead();
            }
        }
    }
}