using System;
using UnityEngine;

namespace CodeBase.Data.PlayerProgress.Npc
{
    [Serializable]
    public class NpcCharacteristics : IDataHealth
    {
        [field: SerializeField] public float PatrolSpeed { get; private set; }
        [field: SerializeField] public float WalkingSpeed { get; private set; }
        [field: SerializeField] public float FollowSpeed { get; private set; }

        [SerializeField] private int _maxHealth;
        [SerializeField] private int _currentHealth;
        public Action OnChangeCurrentHealth { get; set; }

        public int MaxHealth
        {
            get => _maxHealth;
            private set
            {
                _maxHealth = value;
                if (_maxHealth < 0)
                    _maxHealth = 0;
            }
        }

        public int CurrentHealth
        {
            get => _currentHealth;
            private set
            {
                _currentHealth = value;
                if (_currentHealth < 0)
                    _currentHealth = 0;
            }
        }

        public NpcCharacteristics(int maxHealth, int currentHealth, float patrolSpeed, float walkingSpeed, float followSpeed)
        {
            _maxHealth = maxHealth;
            _currentHealth = currentHealth;

            PatrolSpeed = patrolSpeed;
            WalkingSpeed = walkingSpeed;
            FollowSpeed = followSpeed;
        }

        /// <summary>
        ///     takes a positive number
        /// </summary>
        public void Decrement(int value)
        {
            if (value < 0)
            {
                Debug.LogError("it seems the value is minus");
                return;
            }

            CurrentHealth -= value;
            OnChangeCurrentHealth?.Invoke();
        }
    }
}