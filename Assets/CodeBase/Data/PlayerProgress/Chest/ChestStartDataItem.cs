using CodeBase.StaticData.Items;
using System;
using UnityEngine;

namespace CodeBase.Data.PlayerProgress.Chest
{
    [Serializable]
    public class ChestStartDataItem
    {
        public string Name;
        [field: SerializeField] public ItemId ItemId { get; private set; }
        [field: SerializeField] public int Amount { get; private set; } = 1;
    }
}