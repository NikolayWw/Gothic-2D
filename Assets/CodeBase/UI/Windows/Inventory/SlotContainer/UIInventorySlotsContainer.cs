using CodeBase.Data.PlayerProgress.InventoryData;
using CodeBase.UI.Services.Factory;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.UI.Windows.Inventory.SlotContainer
{
    public class UIInventorySlotsContainer : MonoBehaviour
    {
        [SerializeField] private Transform _inventoryWindow;
        private IUIFactory _uiFactory;
        public Action<InventorySlotsContainer, Slot> OnDownClick;
        public Action<InventorySlotsContainer, Slot> OnUpClick;

        public InventorySlotsContainer InventorySlotsContainer { get; private set; }

        public async void Initialize(InventorySlotsContainer inventorySlotsContainer, IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
            InventorySlotsContainer = inventorySlotsContainer;
            await InitSlots();
        }

        private async Task InitSlots()
        {
            for (var i = 0; i < InventorySlotsContainer.Slots.Length; i++)
            {
                var slot = await _uiFactory.CreateSlot(_inventoryWindow);
                slot.UISlotUpdate.Initialize(InventorySlotsContainer.Slots[i]);
                slot.UISlotClickReporter.Initialize(i, this);
            }
        }
    }
}