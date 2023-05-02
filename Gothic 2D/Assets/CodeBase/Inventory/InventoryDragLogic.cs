using CodeBase.Data.PlayerProgress.InventoryData;
using CodeBase.Services.GameFactory;
using CodeBase.Services.LogicFactory;
using CodeBase.Services.StaticData;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Window;
using CodeBase.UI.Windows.Inventory;
using CodeBase.UI.Windows.Inventory.SlotContainer;
using CodeBase.UI.Windows.Inventory.UseInventoryButtons;
using System.Threading.Tasks;

namespace CodeBase.Inventory
{
    public class InventoryDragLogic
    {
        private readonly IUIFactory _uiFactory;
        private readonly IWindowService _windowService;
        private readonly IStaticDataService _staticDataService;
        private readonly IGameFactory _gameFactory;
        private readonly ILogicFactoryService _logicFactory;

        private DragItemInSlot _dragItemInSlot;
        private ItemDescription _itemDescription;
        private Shop _shop;

        private InventoryWindow _inventoryWindow;
        public bool IsInventoryOpen { get; private set; }

        public InventoryDragLogic(IUIFactory uiFactory, IWindowService windowService, IStaticDataService staticDataService, IGameFactory gameFactory, ILogicFactoryService logicFactory)
        {
            _uiFactory = uiFactory;
            _windowService = windowService;
            _staticDataService = staticDataService;
            _gameFactory = gameFactory;
            _logicFactory = logicFactory;
        }

        public async Task OpenInventory(InventorySlotsContainer boxInventorySlotsContainer = null)
        {
            _logicFactory.CloseGameWindows.Close();

            await _windowService.Open(WindowId.Inventory);
            await _windowService.Open(WindowId.UseInventoryButtons);
            if (_windowService.TryGetWindow(WindowId.Inventory, out _inventoryWindow) == false)
                return;

            InitInventoryLogic(boxInventorySlotsContainer, _inventoryWindow.GetComponent<UIInventorySlotsContainer>());

            _inventoryWindow.OnClosed += CloseInventoryWindow;
            IsInventoryOpen = true;
        }

        public async Task OpenBox(InventorySlotsContainer boxSlotsContainer)
        {
            await OpenInventory(boxSlotsContainer);
            await _windowService.Open(WindowId.Box);

            if (_windowService.TryGetWindow(WindowId.Box, out InventoryWindow window) == false)
                return;

            UIInventorySlotsContainer boxContainer = window.GetComponent<UIInventorySlotsContainer>();
            boxContainer.Initialize(boxSlotsContainer, _uiFactory);

            _dragItemInSlot.AddBox(boxContainer);
            _itemDescription.Add(boxContainer.InventorySlotsContainer);
        }

        public void CloseInventoryWindow()
        {
            _inventoryWindow.OnClosed -= CloseInventoryWindow;

            _dragItemInSlot?.Clean();
            _itemDescription?.Clean();
            _shop?.Clean();

            _dragItemInSlot = null;
            _itemDescription = null;
            _shop = null;
            _inventoryWindow = null;

            _windowService.Close(WindowId.Box);
            _windowService.Close(WindowId.Inventory);
            _windowService.Close(WindowId.UseInventoryButtons);

            IsInventoryOpen = false;
        }

        private void InitInventoryLogic(InventorySlotsContainer boxSlotsContainer, UIInventorySlotsContainer playerInventoryContainer)
        {
            _dragItemInSlot = new DragItemInSlot(playerInventoryContainer, _logicFactory);
            _itemDescription = new ItemDescription(_windowService, playerInventoryContainer.InventorySlotsContainer, _logicFactory);

            if (IsShopInventory(boxSlotsContainer))
                if (_windowService.TryGetWindow(WindowId.UseInventoryButtons, out UseItemInInventoryButtonsWindow window))
                    _shop = new Shop(_logicFactory, boxSlotsContainer, playerInventoryContainer.InventorySlotsContainer, window, _gameFactory, _staticDataService);
        }

        private static bool IsShopInventory(InventorySlotsContainer boxSlotsContainer) =>
            boxSlotsContainer != null && boxSlotsContainer.InventoryType == InventoryType.Shop;
    }
}