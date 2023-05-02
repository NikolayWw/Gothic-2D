using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.StaticData.Player
{
    [CreateAssetMenu(fileName = "New player data", menuName = "Static Data/Player Data")]
    public class PlayerStaticData : ScriptableObject
    {
        [field: SerializeField] public int MaxHealth { get; private set; } = 1;
        [field: SerializeField] public int CurrentHealth { get; private set; } = 1;
        [field: SerializeField] public int Strength { get; private set; } = 1;
        [field: SerializeField] public float AttackDelay { get; private set; }
        [field: SerializeField] public float FindDialogsBoxSize { get; private set; }
        [field: SerializeField] public int Experience { get; private set; }
        [field: SerializeField] public int LP { get; private set; }
        [field: SerializeField] public int Level { get; private set; }
        [field: SerializeField] public List<ExperienceForNextLevel> ExperienceForNextLevel { get; private set; }
        [field: SerializeField] public IncrementLevelUpCharacteristics IncrementLevelUpCharacteristics { get; private set; }

        private void OnValidate()
        {
            if (MaxHealth < 1) MaxHealth = 1;
            if (CurrentHealth < 1) CurrentHealth = 1;

            for (int i = 0; i < ExperienceForNextLevel.Count; i++)
            {
                ExperienceForNextLevel[i].OnValidate(i);
            }
        }
    }

    [Serializable]
    public class ExperienceForNextLevel
    {
        [SerializeField] private string _levelName = string.Empty;
        [field: SerializeField] public int Experience { get; private set; }

        public void OnValidate(int indexInCollection)
        {
            _levelName = indexInCollection.ToString();
        }
    }

    [Serializable]
    public class IncrementLevelUpCharacteristics
    {
        [field: SerializeField] public int Health { get; private set; }
        [field: SerializeField] public int Strength { get; private set; }
        [field: SerializeField] public int LP { get; private set; }
    }
}