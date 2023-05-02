using CodeBase.Infrastructure.Logic;
using CodeBase.Inventory;
using CodeBase.Services.GameFactory;
using CodeBase.Services.StaticData;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Window;

namespace CodeBase.Services.LogicFactory
{
    public class LogicFactoryService : ILogicFactoryService
    {
        public InventoryDragLogic InventoryDragLogic { get; private set; }
        public InventorySlotsHandler InventorySlotsHandler { get; private set; }
        public CloseGameWindows CloseGameWindows { get; private set; }

        public void NewLogicContainerService(IUIFactory uiFactory, IWindowService windowService, IStaticDataService staticDataService, IGameFactory gameFactory) =>
            InventoryDragLogic = new InventoryDragLogic(uiFactory, windowService, staticDataService, gameFactory, this);

        public void NewInventorySlotsHandler(IStaticDataService dataService, IGameFactory gameFactory) =>
            InventorySlotsHandler = new InventorySlotsHandler(dataService, gameFactory);

        public void NewCloseGameWindows(IWindowService windowService, WindowId[] whiteIds) =>
            CloseGameWindows = new CloseGameWindows(windowService, whiteIds);

        public void Clean()
        {
            InventoryDragLogic = null;
            InventorySlotsHandler = null;
            CloseGameWindows = null;
        }
    }
}