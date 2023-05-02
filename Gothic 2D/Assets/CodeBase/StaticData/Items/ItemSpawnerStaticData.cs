using System;
using UnityEngine;

namespace CodeBase.StaticData.Items
{
    [Serializable]
    public class ItemSpawnerStaticData
    {
        [SerializeField] private string _name = string.Empty;
        [field: SerializeField] public string UniqueId { get; private set; }
        [field: SerializeField] public ItemId Id { get; private set; }
        [field: SerializeField] public int Amount { get; private set; }
        [field: SerializeField] public Vector2 Position { get; private set; }

        public ItemSpawnerStaticData(in string uniqueId, ItemId id, int amount, Vector2 position)
        {
            _name = id.ToString();
            UniqueId = uniqueId;
            Id = id;
            Position = position;
            Amount = amount;
        }
    }
}