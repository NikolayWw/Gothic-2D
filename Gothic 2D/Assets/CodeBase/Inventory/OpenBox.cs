#region

using CodeBase.Data.PlayerProgress.Chest;
using CodeBase.Data.PlayerProgress.InventoryData;
using CodeBase.Services.LogicFactory;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Inventory;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace CodeBase.Inventory
{
    public class OpenBox : MonoBehaviour
    {
        [SerializeField] private InventoryType _inventoryType;
        [SerializeField] private List<ChestStartDataItem> _startItems;

        private InventoryConfig _inventoryConfig;
        private ChestData _chestData;
        private InventoryDragLogic _inventoryDragLogic;
        private InventorySlotsHandler _slotsHandler;

        private string _inventoryDataKey;

        public void Construct(in string inventoryKey, ILogicFactoryService logicFactory, IPersistentProgressService persistentProgressService, IStaticDataService dataService)
        {
            _inventoryDragLogic = logicFactory.InventoryDragLogic;
            _slotsHandler = logicFactory.InventorySlotsHandler;
            _chestData = persistentProgressService.PlayerProgress.ChestData;
            _inventoryConfig = dataService.InventoryConfig;
            _inventoryDataKey = inventoryKey;
        }

        private void OnValidate()
        {
            _startItems.ForEach(x => x.Name = x.ItemId.ToString());
        }

        public async void Open()
        {
            if (_inventoryDragLogic.IsInventoryOpen)
                return;

            await _inventoryDragLogic.OpenBox(GetInventoryOrNew());
        }

        private InventorySlotsContainer GetInventoryOrNew()
        {
            return _chestData.ChestDataDictionary.Dictionary.TryGetValue(_inventoryDataKey, out var data)
                ? data
                : NewInventory();
        }

        private InventorySlotsContainer NewInventory()
        {
            var inventory = new InventorySlotsContainer(_inventoryConfig.SlotCount, _inventoryType);
            inventory.InitSlots();
            foreach (var itemConfig in _startItems)
            {
                var amount = itemConfig.Amount;
                _slotsHandler.Add(itemConfig.ItemId, ref amount, inventory);
            }

            _chestData.ChestDataDictionary.Dictionary[_inventoryDataKey] = inventory;
            return inventory;
        }
    }
}