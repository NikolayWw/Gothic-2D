using CodeBase.Data.PlayerProgress.InventoryData;
using CodeBase.Services.LogicFactory;
using CodeBase.UI.Services.Window;
using CodeBase.UI.Windows.Inventory;

namespace CodeBase.Inventory
{
    public class ItemDescription
    {
        private readonly IWindowService _windowService;
        private readonly InventorySlotsContainer _playerInventory;
        private InventorySlotsContainer _boxInventory;
        private readonly InventorySlotsHandler _slotsHandler;

        private InventorySlotsContainer _currentInventory;

        public ItemDescription(IWindowService windowService, InventorySlotsContainer playerInventory, ILogicFactoryService logicFactory)
        {
            _slotsHandler = logicFactory.InventorySlotsHandler;
            _windowService = windowService;
            _playerInventory = playerInventory;
            _playerInventory.OnChangeValueSlot += UpdateDescription;
        }

        public void Add(InventorySlotsContainer boxInventorySlotsContainer)
        {
            _boxInventory = boxInventorySlotsContainer;
            _boxInventory.OnChangeValueSlot += UpdateDescription;
        }

        public void Clean()
        {
            _playerInventory.OnChangeValueSlot -= UpdateDescription;
            if (_boxInventory != null)
                _boxInventory.OnChangeValueSlot -= UpdateDescription;

            _windowService.Close(WindowId.ItemDescription);
        }

        private async void UpdateDescription()
        {
            if (IsSlotFrameEnable(_playerInventory) || IsSlotFrameEnable(_boxInventory))
            {
                var itemDescriptionWindow = await _windowService.GetOrOpenWindow<ItemDescriptionWindow>(WindowId.ItemDescription);
                itemDescriptionWindow.UpdateDescription(_currentInventory, _currentInventory.CurrentFrameSlot, IsShopOpen());
            }
            else
                _windowService.Close(WindowId.ItemDescription);
        }

        private bool IsShopOpen() =>
            _boxInventory?.InventoryType == InventoryType.Shop;

        private bool IsSlotFrameEnable(InventorySlotsContainer inventory)
        {
            _currentInventory = inventory;
            return _slotsHandler.IsFrameEnableAndItemPresent(inventory, out _);
        }
    }
}