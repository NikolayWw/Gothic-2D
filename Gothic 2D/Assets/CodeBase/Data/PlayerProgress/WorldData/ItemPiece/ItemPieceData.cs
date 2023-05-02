using System;
using CodeBase.StaticData.Items;
using UnityEngine;

namespace CodeBase.Data.PlayerProgress.WorldData.ItemPiece
{
    [Serializable]
    public class ItemPieceData
    {
        [field: SerializeField] public string UniqueId { get; private set; }
        [field: SerializeField] public ItemId Id { get; private set; }
        [field: SerializeField] public int Amount { get; private set; }
        [field: SerializeField] public Vector3 Position { get; private set; }

        public ItemPieceData(string uniqueId, ItemId id, int amount, Vector3 position)
        {
            UniqueId = uniqueId;
            Id = id;
            Amount = amount;
            Position = position;
        }
    }
}