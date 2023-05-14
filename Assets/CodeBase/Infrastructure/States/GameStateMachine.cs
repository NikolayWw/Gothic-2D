using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Logic;
using CodeBase.Services;
using CodeBase.Services.GameFactory;
using CodeBase.Services.LogicFactory;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Window;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeBase.Infrastructure.States
{
    public class GameStateMachine : IGameStateMachine
    {
        private Dictionary<Type, IExitable> _states;
        private IExitable _activeState;

        public async Task InitStateMachine(SceneLoader sceneLoader, LoadCurtain loadCurtain, AllServices services, ICoroutineRunner coroutineRunner)
        {
            _states = new Dictionary<Type, IExitable>
            {
                [typeof(BootstrapState)] = await RegisterServices(services, coroutineRunner),
                [typeof(LoopState)] = new LoopState(),
                [typeof(LoadProgressState)] = new LoadProgressState(this, services.Single<ISaveLoadService>()),

                [typeof(LoadMainMenuState)] = new LoadMainMenuState(sceneLoader, loadCurtain, this,
                    services.Single<IAssetProvider>(),
                    services.Single<IUIFactory>(),
                    services.Single<IWindowService>(),
                    services.Single<ILogicFactoryService>()),

                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, loadCurtain,
                    services.Single<IGameFactory>(),
                    services.Single<IUIFactory>(),
                    services.Single<IPersistentProgressService>(),
                    services.Single<IWindowService>(),
                    services.Single<IStaticDataService>(),
                    services.Single<ILogicFactoryService>(),
                    services.Single<IAssetProvider>()),
            };
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
        {
            var state = ChangeState<TState>();
            state.Enter(payload);
        }

        public void Enter<TState>() where TState : class, IState
        {
            var state = ChangeState<TState>();
            state.Enter();
        }

        private TState ChangeState<TState>() where TState : class, IExitable
        {
            _activeState?.Exit();
            var state = _states[typeof(TState)] as TState;
            _activeState = state;
            return state;
        }

        private async Task<BootstrapState> RegisterServices(AllServices services, ICoroutineRunner coroutineRunner)
        {
            var bootstrapState = new BootstrapState(this, services);
            await bootstrapState.RegisterServices();
            return bootstrapState;
        }
    }
}