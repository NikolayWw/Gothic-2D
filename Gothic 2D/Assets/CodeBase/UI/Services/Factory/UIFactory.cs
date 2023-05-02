using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.States;
using CodeBase.Player;
using CodeBase.Player.UseItems;
using CodeBase.Services.GameFactory;
using CodeBase.Services.LogicFactory;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Window;
using CodeBase.UI.Windows;
using CodeBase.UI.Windows.Ads;
using CodeBase.UI.Windows.Characteristics;
using CodeBase.UI.Windows.Dialog;
using CodeBase.UI.Windows.GameMenu;
using CodeBase.UI.Windows.GamePlayMessage;
using CodeBase.UI.Windows.Input;
using CodeBase.UI.Windows.Inventory;
using CodeBase.UI.Windows.Inventory.SlotContainer;
using CodeBase.UI.Windows.Inventory.UseInventoryButtons;
using CodeBase.UI.Windows.LoadSaveMenu;
using CodeBase.UI.Windows.MainMenu;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Services.Ads;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootAddress = "UIRoot";

        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly ILogicFactoryService _logicFactory;
        private readonly IGameFactory _gameFactory;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IAdsService _adsService;

        private Transform _uiRoot;
        public Dictionary<WindowId, BaseWindow> WindowsContainer { get; } = new Dictionary<WindowId, BaseWindow>();

        public UIFactory(IAssetProvider assetProvider, IStaticDataService staticDataService, IPersistentProgressService persistentProgressService, ILogicFactoryService logicFactory, IGameFactory gameFactory, ISaveLoadService saveLoadService, IGameStateMachine gameStateMachine,IAdsService adsService)
        {
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
            _persistentProgressService = persistentProgressService;
            _logicFactory = logicFactory;
            _gameFactory = gameFactory;
            _saveLoadService = saveLoadService;
            _gameStateMachine = gameStateMachine;
            _adsService = adsService;
        }

        public void Clean()
        {
            WindowsContainer.Clear();
        }

        public async Task Warmup()
        {
            await _assetProvider.Load<GameObject>(_staticDataService.ForWindow(WindowId.Inventory).Template);
            await _assetProvider.Load<GameObject>(_staticDataService.ForWindow(WindowId.Box).Template);
            await _assetProvider.Load<GameObject>(_staticDataService.ForWindow(WindowId.LoadSaveMenu).Template);
            await _assetProvider.Load<GameObject>(_staticDataService.ForWindow(WindowId.Characteristics).Template);
            await _assetProvider.Load<GameObject>(_staticDataService.ForWindow(WindowId.GameMenu).Template);

            await _assetProvider.Load<GameObject>(_staticDataService.UILoadSaveStaticData.SaveInfoDescriptionReference);
            await _assetProvider.Load<GameObject>(_staticDataService.UILoadSaveStaticData.LoadSaveButtonReference);
        }

        public async Task CreateUIRoot()
        {
            var instantiate = await _assetProvider.Instantiate(UIRootAddress);
            _uiRoot = instantiate.transform;
        }

        public async Task CreateInventory(IWindowService windowService)
        {
            var inventoryWindow = await InstantiateRegister<InventoryWindow>(WindowId.Inventory);
            inventoryWindow.GetComponent<UIInventorySlotsContainer>()?.Initialize(_persistentProgressService.PlayerProgress.PlayerData.SlotsContainer, this);
        }

        public async Task CreateBox()
        {
            await InstantiateRegister<InventoryWindow>(WindowId.Box);
        }

        public async Task<GameObject> CreateSaveInfoDescription()
        {
            var prefab = await _assetProvider.Load<GameObject>(_staticDataService.UILoadSaveStaticData.SaveInfoDescriptionReference);
            var instance = Object.Instantiate(prefab);
            return instance;
        }

        public async Task CreateInput()
        {
            var window = await InstantiateRegister<InputWindow>(WindowId.Input);
        }

        public async Task<LoadSaveButton> CreateLoadSaveButton(IWindowService windowService, Transform parent)
        {
            var prefab = await _assetProvider.Load<GameObject>(_staticDataService.UILoadSaveStaticData.LoadSaveButtonReference);
            LoadSaveButton instance = Object.Instantiate(prefab, parent).GetComponent<LoadSaveButton>();
            instance.Construct(windowService);
            return instance;
        }

        public async Task CreateLoadSaveMenu(IWindowService windowService)
        {
            var window = await InstantiateRegister<LoadSaveMenuWindow>(WindowId.LoadSaveMenu);
            window.Construct(_staticDataService, _saveLoadService, this, windowService);
            window.GetComponentInChildren<CloseSaveMenuButton>()?.Construct(windowService);
        }

        public async Task CreateUseItemButtonsInInventory()
        {
            var window = await InstantiateRegister<UseItemInInventoryButtonsWindow>(WindowId.UseInventoryButtons);
            window.GetComponent<DropItem>()?.Construct(_persistentProgressService, _logicFactory);

            UseItemsInInventoryContainer useItemsInInventoryContainer = _gameFactory.Player.GetComponent<UseItemsInInventoryContainer>();
            window.GetComponent<EquipButton>()?.Construct(_persistentProgressService, _logicFactory, useItemsInInventoryContainer.EquipItems);
            window.GetComponent<ConsumeButton>()?.Construct(_persistentProgressService, _logicFactory, useItemsInInventoryContainer.ConsumeItems);
        }

        public async Task CreateItemDescription(WindowService windowService)
        {
            ItemDescriptionWindow window = await InstantiateRegister<ItemDescriptionWindow>(WindowId.ItemDescription);
            window.Construct(_staticDataService);
        }

        public async Task<SlotComponents> CreateSlot(Transform parent)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(_staticDataService.InventoryConfig.SlotComponents);
            SlotComponents slotComponents = Object.Instantiate(prefab, parent).GetComponent<SlotComponents>();
            slotComponents.UISlotUpdate.Construct(_staticDataService);
            return slotComponents;
        }

        public async Task CreateCharacteristics()
        {
            var window = await InstantiateRegister<CharacteristicsWindow>(WindowId.Characteristics);
            window.Construct(_persistentProgressService);
        }

        public async Task<DialogBuilder> CreateDialogWindow()
        {
            var prefab = await _assetProvider.Load<GameObject>(_staticDataService.DialogStaticData.DialogWindowReference);
            DialogBuilder dialogBuilder = Object.Instantiate(prefab, _uiRoot).GetComponent<DialogBuilder>();
            dialogBuilder.Construct(_staticDataService, this);
            return dialogBuilder;
        }

        public async Task CreateSaveInfoPanel()
        {
            var window = await InstantiateRegister<SaveDescriptionPanel>(WindowId.SaveDescriptionPanel);
            window.Construct(_gameStateMachine, _saveLoadService, _persistentProgressService);
        }

        public async Task CreateGamePlayMessageWindow()
        {
            GamePlayMessageWindow window = await InstantiateRegister<GamePlayMessageWindow>(WindowId.GameplayMessage);
            window.Construct(this, _persistentProgressService);
        }

        public async Task CreateGameplayMessage(string context, Transform parent)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(_staticDataService.GameplayMessageData.MessageReference);
            GameplayMessage instantiate = Object.Instantiate(prefab, parent).GetComponent<GameplayMessage>();
            instantiate.Construct(context, _staticDataService.GameplayMessageData.LifeTime);
        }

        public async Task CreateGameMenu(IWindowService windowService)
        {
            var window = await InstantiateRegister<GameMenuWindow>(WindowId.GameMenu);
            window.Construct();

            window.GetComponentInChildren<LoadMainMenuButton>()?.Construct(_gameStateMachine);
            foreach (var openWindowButton in window.GetComponentsInChildren<OpenWindowButton>())
                openWindowButton.Construct(windowService, _logicFactory);
        }

        public async Task CreateAdsWindow()
        {
            RewardedAddMoneyWindow rewardedAddMoney = await InstantiateRegister<RewardedAddMoneyWindow>(WindowId.Ads);
            rewardedAddMoney.Construct(_staticDataService, _adsService, _logicFactory, _persistentProgressService);
        }

        public async Task CreateMainMenu(IWindowService windowService)
        {
            var window = await InstantiateRegister<MainMenuWindow>(WindowId.MainMenu);
            window.GetComponentInChildren<LoadNewGameButton>()?.Construct(_saveLoadService, _gameStateMachine, _persistentProgressService);

            foreach (var openWindowButton in window.GetComponentsInChildren<OpenWindowButton>())
                openWindowButton.Construct(windowService, _logicFactory);
        }

        #region Buttons

        public async Task CreateOpenGameMenuButton(IWindowService windowService)
        {
            var window = await InstantiateRegister<OpenWindowButton>(WindowId.OpenGameMenuButton);
            window.Construct(windowService, _logicFactory);
        }

        public async Task<DialogButton> CreateDialogButton(Transform parent, string buttonName)
        {
            var prefab = await _assetProvider.Load<GameObject>(_staticDataService.DialogStaticData.DialogButtonReference);
            DialogButton button = Object.Instantiate(prefab, parent).GetComponent<DialogButton>();
            button.Text.text = buttonName;
            return button;
        }

        public async Task CreateOpenCharacteristicsButton(IWindowService windowService)
        {
            var button = await InstantiateRegister<OpenWindowButton>(WindowId.OpenCharacteristicsButton);
            button.Construct(windowService, _logicFactory);
        }

        public async Task CreateOpenInventoryButton()
        {
            var button = await InstantiateRegister<OpenInventoryButton>(WindowId.OpenInventoryButton);
            button.Construct(_logicFactory);
        }

        public async Task CreatePlayerInteractionButton()
        {
            await InstantiateRegister<PlayerInteractionButton>(WindowId.PlayerInteractsButton);
        }

        public async Task CreatePlayerHitButton(PlayerKiller playerKiller)
        {
            PlayerHitButton button = await InstantiateRegister<PlayerHitButton>(WindowId.PlayerHitButton);
            button.Construct(playerKiller);
        }

        #endregion Buttons

        private async Task<TWindow> InstantiateRegister<TWindow>(WindowId id) where TWindow : BaseWindow
        {
            WindowConfig config = _staticDataService.ForWindow(id);
            var prefab = await _assetProvider.Load<GameObject>(config.Template);
            BaseWindow window = Object.Instantiate(prefab, _uiRoot).GetComponent<BaseWindow>();

            window.SetId(id);
            WindowsContainer[id] = window;
            return (TWindow)window;
        }
    }
}