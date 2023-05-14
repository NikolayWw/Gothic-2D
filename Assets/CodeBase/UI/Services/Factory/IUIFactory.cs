#region

using CodeBase.Player;
using CodeBase.Services;
using CodeBase.UI.Services.Window;
using CodeBase.UI.Windows;
using CodeBase.UI.Windows.Dialog;
using CodeBase.UI.Windows.Inventory.SlotContainer;
using CodeBase.UI.Windows.LoadSaveMenu;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

#endregion

namespace CodeBase.UI.Services.Factory
{
    public interface IUIFactory : IService
    {
        Task CreateUIRoot();

        Dictionary<WindowId, BaseWindow> WindowsContainer { get; }

        void Clean();

        Task<SlotComponents> CreateSlot(Transform parent);

        Task CreateOpenInventoryButton();

        Task CreateItemDescription(WindowService windowService);

        Task CreateInventory(IWindowService windowService);

        Task CreatePlayerInteractionButton();

        Task CreateBox();

        Task CreateUseItemButtonsInInventory();

        Task<DialogButton> CreateDialogButton(Transform parent, string buttonName);

        Task<DialogBuilder> CreateDialogWindow();

        Task CreateCharacteristics();

        Task CreateOpenCharacteristicsButton(IWindowService windowService);

        Task CreateGamePlayMessageWindow();

        Task CreateGameplayMessage(string context, Transform parent);

        Task CreateMainMenu(IWindowService windowService);

        Task CreateLoadSaveMenu(IWindowService windowService);

        Task CreateSaveInfoPanel();

        Task Warmup();

        Task<LoadSaveButton> CreateLoadSaveButton(IWindowService windowService, Transform parent);

        Task<GameObject> CreateSaveInfoDescription();

        Task CreateOpenGameMenuButton(IWindowService windowService);

        Task CreateGameMenu(IWindowService windowService);

        Task CreateInput();

        Task CreatePlayerHitButton(PlayerKiller playerKiller);
        Task CreateAdsWindow();
    }
}