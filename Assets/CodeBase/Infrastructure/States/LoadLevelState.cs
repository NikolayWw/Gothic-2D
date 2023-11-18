using CodeBase.Data.PlayerProgress.WorldData.ItemPiece;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Logic;
using CodeBase.Logic;
using CodeBase.Player;
using CodeBase.Services.GameFactory;
using CodeBase.Services.LogicFactory;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Items;
using CodeBase.StaticData.Level;
using CodeBase.StaticData.Npc;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Window;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadState<string>
    {
        private readonly WindowId[] WhiteCloseWindowsIds =
        {
            WindowId.Characteristics,
            WindowId.Inventory,
            WindowId.Box,
            WindowId.GameMenu,
            WindowId.LoadSaveMenu,
        };

        private readonly IStaticDataService _staticDataService;
        private readonly ILogicFactoryService _logicFactory;
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;
        private readonly IUIFactory _uiFactory;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly IWindowService _windowService;
        private readonly IAssetProvider _assetProvider;

        private SceneComponentContainer _componentContainer;

        public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadCurtain loadingCurtain, IGameFactory gameFactory, IUIFactory uiFactory, IPersistentProgressService persistentProgressService, IWindowService windowService, IStaticDataService staticDataService, ILogicFactoryService logicFactory, IAssetProvider assetProvider)
        {
            _staticDataService = staticDataService;
            _logicFactory = logicFactory;
            _assetProvider = assetProvider;
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _uiFactory = uiFactory;
            _persistentProgressService = persistentProgressService;
            _windowService = windowService;
        }

        public async void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            Clean();
            await Warmup();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            _loadingCurtain.Hide();
        }

        private async void OnLoaded()
        {
            LevelStaticData data = _staticDataService.ForLevel(SceneManager.GetActiveScene().name);
            await InitGameLogic();
            await InitWorld(data);
            InformLoadProgress();
            _stateMachine.Enter<LoopState>();
        }

        private async Task InitWorld(LevelStaticData levelData)
        {
            InitChests();
            await InitNpcSpawners(levelData);
            await InitItemSpawners(levelData);
            await InitItemPiece();

            PlayerInteractionButton playerInteractionButton = await _windowService.GetOrOpenWindow<PlayerInteractionButton>(WindowId.PlayerInteractsButton);
            GameObject player = await InitPlayer(levelData, playerInteractionButton);
            await InitCamera(player);
            await _uiFactory.CreatePlayerHitButton(player.GetComponent<PlayerKiller>());
        }

        private async Task InitGameLogic()
        {
            await _uiFactory.CreateUIRoot();

            FindComponentContainer();
            _logicFactory.NewLogicContainerService(_uiFactory, _windowService, _staticDataService, _gameFactory);
            _logicFactory.NewInventorySlotsHandler(_staticDataService, _gameFactory);
            _logicFactory.NewCloseGameWindows(_windowService, WhiteCloseWindowsIds);

            await _windowService.Open(WindowId.Input);
            await _windowService.Open(WindowId.OpenInventoryButton);
            await _windowService.Open(WindowId.OpenCharacteristicsButton);
            await _windowService.Open(WindowId.GameplayMessage);
            await _windowService.Open(WindowId.OpenGameMenuButton);
           // await _windowService.Open(WindowId.Ads);
        }

        private void InformLoadProgress()
        {
            _gameFactory.SaveProgress.ForEach(x => x.LoadProgress(_persistentProgressService.PlayerProgress));
        }

        private async Task InitItemSpawners(LevelStaticData levelStaticData)
        {
            foreach (ItemSpawnerStaticData data in levelStaticData.ItemSpawnerStaticDatas)
                await _gameFactory.CreateItemSpawner(data.UniqueId, data.Id, data.Amount, data.Position);
        }

        private async Task InitItemPiece()
        {
            ItemPieceDataDictionary pieceDataDictionary = _persistentProgressService.PlayerProgress.WorldData.ItemPieceDataDictionary;

            Dictionary<string, ItemPieceData> closeItemDictionary = pieceDataDictionary.Dictionary.ToDictionary(x => x.Key, x => new ItemPieceData(x.Value.UniqueId, x.Value.Id, x.Value.Amount, x.Value.Position));
            pieceDataDictionary.Dictionary.Clear();

            foreach (var itemPieceData in closeItemDictionary.Values)
            {
                var itemPiece = await _gameFactory.CreateItemPiece(itemPieceData.Id, itemPieceData.Amount, itemPieceData.Position);
                string uniqueId = itemPieceData.UniqueId;
                itemPiece.Initialize(uniqueId);
            }
        }

        private void InitChests() =>
            _componentContainer.Boxes.ForEach(x => x.Initialize(_logicFactory, _persistentProgressService, _staticDataService));

        private async Task<GameObject> InitPlayer(LevelStaticData levelData, PlayerInteractionButton playerInteractionButton)
        {
            GameObject player = await _gameFactory.CreatePlayer(levelData.PlayerInitialPoint, _windowService, playerInteractionButton);
            return player;
        }

        private async Task InitCamera(GameObject player)
        {
            await _gameFactory.CreateCMVcam(player.transform, _componentContainer.CameraConfinerCollider);
        }

        private async Task InitNpcSpawners(LevelStaticData levelData)
        {
            foreach (NpcSpawnerStaticData data in levelData.NpcSpawnerStaticData)
                await _gameFactory.CreateNpcSpawner(data.Id, data.Position, _uiFactory, _windowService);
        }

        private void Clean()
        {
            _gameFactory?.Clean();
            _uiFactory?.Clean();
            _persistentProgressService?.PlayerProgress?.QuestContainer?.Unsubscribe();
            _assetProvider.Cleanup();
        }

        private async Task Warmup()
        {
            await _gameFactory.Warmup();
            await _uiFactory.Warmup();
        }

        private void FindComponentContainer()
        {
            _componentContainer = Object.FindObjectOfType<SceneComponentContainer>();
        }
    }
}