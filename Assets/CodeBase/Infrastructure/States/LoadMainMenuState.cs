using CodeBase.Data;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Logic;
using CodeBase.Services.LogicFactory;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Window;

namespace CodeBase.Infrastructure.States
{
    public class LoadMainMenuState : IState
    {
        private readonly WindowId[] WhiteCloseWindowIds = { WindowId.MainMenu, WindowId.LoadSaveMenu };

        private readonly SceneLoader _sceneLoader;
        private readonly LoadCurtain _loadCurtain;
        private readonly IAssetProvider _assetProvider;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IUIFactory _uiFactory;
        private readonly IWindowService _windowService;
        private readonly ILogicFactoryService _logicFactory;

        public LoadMainMenuState(SceneLoader sceneLoader, LoadCurtain loadCurtain, IGameStateMachine gameStateMachine, IAssetProvider assetProvider, IUIFactory uiFactory, IWindowService windowService, ILogicFactoryService logicFactory)
        {
            _sceneLoader = sceneLoader;
            _loadCurtain = loadCurtain;
            _assetProvider = assetProvider;
            _gameStateMachine = gameStateMachine;
            _uiFactory = uiFactory;
            _windowService = windowService;
            _logicFactory = logicFactory;
        }
        
        public async void Enter()
        {
            _loadCurtain.Show();
            Cleanup();
            await _uiFactory.Warmup();
            _sceneLoader.Load(GameConstants.MainMenuScene, LoadMainMenu);
        }

        public void Exit()
        {
            _loadCurtain.Hide();
        }

        private async void LoadMainMenu()
        {
            await _uiFactory.CreateUIRoot();
            _logicFactory.NewCloseGameWindows(_windowService, WhiteCloseWindowIds);

            await _windowService.Open(WindowId.MainMenu);
            _gameStateMachine.Enter<LoopState>();
        }

        private void Cleanup()
        {
            _assetProvider.Cleanup();
            _logicFactory.Clean();
        }
    }
}