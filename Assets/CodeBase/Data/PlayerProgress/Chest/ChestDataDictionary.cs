using CodeBase.Data.PlayerProgress.InventoryData;
using System;

namespace CodeBase.Data.PlayerProgress.Chest
{
    [Serializable]
    public class ChestDataDictionary : SerializableDictionary<string, InventorySlotsContainer>
    { }
}