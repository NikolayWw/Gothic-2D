using CodeBase.Data.PlayerProgress.InventoryData;
using System;

namespace CodeBase.Data.PlayerProgress.Player
{
    [Serializable]
    public class PlayerData
    {
        public InventorySlotsContainer SlotsContainer;
        public PlayerCharacteristics PlayerCharacteristics;

        public PlayerData(int inventorySlotCount, InventoryType inventoryType, PlayerCharacteristics playerCharacteristics)
        {
            SlotsContainer = new InventorySlotsContainer(inventorySlotCount, inventoryType);
            PlayerCharacteristics = playerCharacteristics;
        }
    }
}