using CodeBase.UI.Services.Factory;
using CodeBase.UI.Windows;
using System;
using System.Threading.Tasks;

namespace CodeBase.UI.Services.Window
{
    public class WindowService : IWindowService
    {
        public Action<WindowId> OnWindowOpen { get; set; }
        private readonly IUIFactory _uiFactory;

        public WindowService(IUIFactory uiFactory) =>
            _uiFactory = uiFactory;

        public async Task Open(WindowId id)
        {
            switch (id)
            {
                case WindowId.PlayerHitButton:
                case WindowId.None:
                    break;

                case WindowId.Inventory:
                    await _uiFactory.CreateInventory(this);
                    break;

                case WindowId.ItemDescription:
                    await _uiFactory.CreateItemDescription(this);
                    break;

                case WindowId.Box:
                    await _uiFactory.CreateBox();
                    break;

                case WindowId.Characteristics:
                    await _uiFactory.CreateCharacteristics();
                    break;

                case WindowId.OpenCharacteristicsButton:
                    await _uiFactory.CreateOpenCharacteristicsButton(this);
                    break;

                case WindowId.GameplayMessage:
                    await _uiFactory.CreateGamePlayMessageWindow();
                    break;

                case WindowId.MainMenu:
                    await _uiFactory.CreateMainMenu(this);
                    break;

                case WindowId.LoadSaveMenu:
                    await _uiFactory.CreateLoadSaveMenu(this);
                    break;

                case WindowId.SaveDescriptionPanel:
                    await _uiFactory.CreateSaveInfoPanel();
                    break;

                case WindowId.GameMenu:
                    await _uiFactory.CreateGameMenu(this);
                    break;

                case WindowId.Input:
                    await _uiFactory.CreateInput();
                    break;

                case WindowId.UseInventoryButtons:
                    await _uiFactory.CreateUseItemButtonsInInventory();
                    break;

                case WindowId.OpenInventoryButton:
                    await _uiFactory.CreateOpenInventoryButton();
                    break;

                case WindowId.PlayerInteractsButton:
                    await _uiFactory.CreatePlayerInteractionButton();
                    break;

                case WindowId.OpenGameMenuButton:
                    await _uiFactory.CreateOpenGameMenuButton(this);
                    break;

                case WindowId.Ads:
                    await _uiFactory.CreateAdsWindow();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(id), id, null);
            }

            OnWindowOpen?.Invoke(id);
        }

        public async Task<TWindow> GetOrOpenWindow<TWindow>(WindowId id) where TWindow : BaseWindow
        {
            if (TryGetWindow(id, out TWindow getWindow))
                return getWindow;

            await Open(id);
            TryGetWindow(id, out TWindow instanceWindow);
            return instanceWindow;
        }

        public bool IsWindowOpen(WindowId id) =>
            _uiFactory.WindowsContainer.ContainsKey(id);

        public void Close(WindowId id)
        {
            if (TryGetWindow(id, out BaseWindow window))
            {
                window.Close();
                _uiFactory.WindowsContainer.Remove(id);
            }
        }

        public bool TryGetWindow<TWindow>(WindowId id, out TWindow result) where TWindow : BaseWindow
        {
            result = _uiFactory.WindowsContainer.TryGetValue(id, out var window) ? (TWindow)window : null;
            return result;
        }
    }
}