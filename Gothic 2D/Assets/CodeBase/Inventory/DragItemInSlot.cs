#region

using CodeBase.Data.PlayerProgress.InventoryData;
using CodeBase.Services.LogicFactory;
using CodeBase.UI.Windows.Inventory.SlotContainer;

#endregion

namespace CodeBase.Inventory
{
    public class DragItemInSlot
    {
        private readonly UIInventorySlotsContainer _uiSlotsContainer;
        private readonly InventorySlotsHandler _slotsHandler;

        private UIInventorySlotsContainer _boxContainer;
        private InventorySlotsContainer _previousInventory;
        private Slot _previousSlot;

        public DragItemInSlot(UIInventorySlotsContainer uiSlotsContainer, ILogicFactoryService logicFactory)
        {
            _uiSlotsContainer = uiSlotsContainer;
            _slotsHandler = logicFactory.InventorySlotsHandler;
            _uiSlotsContainer.OnDownClick += DownClick;
        }

        public void AddBox(UIInventorySlotsContainer boxContainer)
        {
            _boxContainer = boxContainer;
            _boxContainer.OnDownClick += DownClick;
        }

        public void Clean()
        {
            _uiSlotsContainer.OnDownClick -= DownClick;
            _slotsHandler.DeselectSlots(_uiSlotsContainer.InventorySlotsContainer);

            if (_boxContainer)
            {
                _boxContainer.OnDownClick -= DownClick;
                _slotsHandler.DeselectSlots(_boxContainer.InventorySlotsContainer);
            }
        }

        private void DownClick(InventorySlotsContainer inventory, Slot slot)
        {
            if (_previousSlot == null || (_previousSlot.IsFrameSelected == false || _previousSlot.Equals(slot)))
            {
                slot.SetFrameSelected(!slot.IsFrameSelected);
            }
            else if (_previousInventory.InventoryType == InventoryType.Shop || inventory.InventoryType == InventoryType.Shop || _previousSlot.IsEquip || slot.IsEquip)
            {
                _slotsHandler.DeselectSlots(_previousInventory);
                slot.SetFrameSelected(!slot.IsFrameSelected);
            }
            else if (_previousSlot.IsEmpty == false || slot.IsEmpty == false)
            {
                _slotsHandler.ExchangeSlotData(_previousSlot, slot);
                _previousSlot.SetFrameSelected(false);
            }
            else
            {
                _previousSlot.SetFrameSelected(false);
                slot.SetFrameSelected(true);
            }

            _previousInventory = inventory;
            _previousSlot = slot;
        }
    }
}