using CodeBase.StaticData.Player;
using System;
using UnityEngine;

namespace CodeBase.Data.PlayerProgress.Player
{
    [Serializable]
    public class PlayerCharacteristics : IDataHealth
    {
        private PlayerStaticData _playerStaticData;
        [field: SerializeField] public int LP { get; private set; }
        [field: SerializeField] public int Level { get; private set; }
        [field: SerializeField] public int Experience { get; private set; }
        [field: SerializeField] public int NextLevel { get; private set; }
        [field: SerializeField] public int Strength { get; private set; }
        [field: SerializeField] public float AttackDelay { get; private set; }

        [SerializeField] private int _maxHealth;
        [SerializeField] private int _currentHealth;
        public Action OnChangeCurrentHealth { get; set; }
        public Action OnChangeCharacteristics;
        public Action<int> OnIncrementExperience;

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
                _currentHealth = Mathf.Clamp(_currentHealth, 0, MaxHealth);
            }
        }

        public PlayerCharacteristics(PlayerStaticData playerStaticData)
        {
            _playerStaticData = playerStaticData;
            MaxHealth = _playerStaticData.MaxHealth;
            CurrentHealth = playerStaticData.CurrentHealth;
            Strength = playerStaticData.Strength;
            LP = playerStaticData.LP;
            Experience = playerStaticData.Experience;
            AttackDelay = playerStaticData.AttackDelay;

            if (IsMaxLevel() == false)
                NextLevel = _playerStaticData.ExperienceForNextLevel[Level].Experience;
        }

        public void SetPlayerStaticData(PlayerStaticData data) =>
            _playerStaticData = data;

        public void IncrementExperience(int value)
        {
            Experience += value;
            int stackOverflowCount = 0;

            if (IsMaxLevel() == false)
            {
                while (Experience >= _playerStaticData.ExperienceForNextLevel[Level].Experience)
                {
                    Level++;
                    if (IsMaxLevel() == false)
                        NextLevel = _playerStaticData.ExperienceForNextLevel[Level].Experience;
                    IncrementLevelUpCharacteristics();

                    stackOverflowCount++;
                    if (stackOverflowCount > 10_000)//stackOverflow
                    {
                        Debug.LogError("stackOverflowCount");
                        break;
                    }
                }
            }
            OnIncrementExperience?.Invoke(value);
        }

        /// <summary>
        ///     takes a positive number
        /// </summary>
        public void DecrementHealth(int value)
        {
            if (value < 0)
            {
                Debug.LogError("it seems the value is minus");
                return;
            }

            CurrentHealth -= value;
            OnChangeCurrentHealth?.Invoke();
            OnChangeCharacteristics?.Invoke();
        }

        public void ChangeCharacteristics(int maxHealth = 0, int currentHealth = 0, int strength = 0, int lp = 0)
        {
            MaxHealth += maxHealth;
            CurrentHealth += currentHealth;
            Strength += strength;
            LP += lp;
            OnChangeCurrentHealth?.Invoke();
            OnChangeCharacteristics?.Invoke();
        }

        private void IncrementLevelUpCharacteristics()
        {
            IncrementLevelUpCharacteristics levelUpConfig = _playerStaticData.IncrementLevelUpCharacteristics;

            MaxHealth += levelUpConfig.Health;
            Strength += levelUpConfig.Strength;
            LP += levelUpConfig.LP;
            OnChangeCurrentHealth?.Invoke();
        }

        private bool IsMaxLevel() =>
            _playerStaticData.ExperienceForNextLevel.Count < Level;
    }
}