using System;
using UnityEngine;

namespace CodeBase.Data.PlayerProgress.Chest
{
    [Serializable]
    public class ChestData
    {
        [field: SerializeField] public ChestDataDictionary ChestDataDictionary { get; private set; } = new ChestDataDictionary();
    }
}