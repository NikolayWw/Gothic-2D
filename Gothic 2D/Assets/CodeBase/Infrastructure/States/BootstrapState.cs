using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services;
using CodeBase.Services.Ads;
using CodeBase.Services.GameFactory;
using CodeBase.Services.Input;
using CodeBase.Services.LogicFactory;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Window;
using System.Threading.Tasks;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly AllServices _services;

        public BootstrapState(GameStateMachine stateMachine, AllServices services)
        {
            _stateMachine = stateMachine;
            _services = services;
        }

        public void Enter()
        {
            _stateMachine.Enter<LoadProgressState>();
        }

        public void Exit()
        { }

        public async Task RegisterServices()
        {
            _services.RegisterSingle<IGameStateMachine>(_stateMachine);
            _services.RegisterSingle<IInputService>(new InputService());
            RegisterAds();
            RegisterAssetProvider();
            await RegisterStaticData();
            _services.RegisterSingle<ILogicFactoryService>(new LogicFactoryService());
            _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());

            _services.RegisterSingle<IGameFactory>(new GameFactory(
                _services.Single<IAssetProvider>(),
                _services.Single<IStaticDataService>(),
                _services.Single<IInputService>(),
                _services.Single<IPersistentProgressService>(),
                _services.Single<ILogicFactoryService>()));

            _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(
                _services.Single<IGameFactory>(),
                _services.Single<IPersistentProgressService>(),
                _services.Single<IStaticDataService>()));

            _services.RegisterSingle<IUIFactory>(new UIFactory(_services.Single<IAssetProvider>(),
                _services.Single<IStaticDataService>(),
                _services.Single<IPersistentProgressService>(),
                _services.Single<ILogicFactoryService>(),
                _services.Single<IGameFactory>(),
                _services.Single<ISaveLoadService>(),
                _stateMachine,
                _services.Single<IAdsService>()));

            _services.RegisterSingle<IWindowService>(new WindowService(_services.Single<IUIFactory>()));
        }

        private void RegisterAds()
        {
            var adsService = new AdsService();
            adsService.Initialize();
            _services.RegisterSingle<IAdsService>(adsService);
        }

        private void RegisterAssetProvider()
        {
            var provider = new AssetProvider();
            provider.Initialize();
            _services.RegisterSingle<IAssetProvider>(provider);
        }

        private async Task RegisterStaticData()
        {
            var service = new StaticDataService();
            await service.Load();
            _services.RegisterSingle<IStaticDataService>(service);
        }
    }
}