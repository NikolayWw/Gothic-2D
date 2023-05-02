#region

using CodeBase.Infrastructure.Logic;
using CodeBase.Inventory;
using CodeBase.Services.GameFactory;
using CodeBase.Services.StaticData;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Window;

#endregion

namespace CodeBase.Services.LogicFactory
{
    public interface ILogicFactoryService : IService
    {
        InventoryDragLogic InventoryDragLogic { get; }
        InventorySlotsHandler InventorySlotsHandler { get; }
        CloseGameWindows CloseGameWindows { get; }

        void NewLogicContainerService(IUIFactory uiFactory, IWindowService windowService, IStaticDataService staticDataService, IGameFactory gameFactory);

        void NewInventorySlotsHandler(IStaticDataService dataService, IGameFactory gameFactory);

        void Clean();

        void NewCloseGameWindows(IWindowService windowService, WindowId[] whiteIds);
    }
}