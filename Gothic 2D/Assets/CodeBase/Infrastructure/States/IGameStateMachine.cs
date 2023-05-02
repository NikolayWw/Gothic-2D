using CodeBase.Infrastructure.Logic;
using CodeBase.Services;
using System.Threading.Tasks;

namespace CodeBase.Infrastructure.States
{
    public interface IGameStateMachine : IService
    {
        void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>;

        void Enter<TState>() where TState : class, IState;

        Task InitStateMachine(SceneLoader sceneLoader, LoadCurtain loadCurtain, AllServices services, ICoroutineRunner coroutineRunner);
    }
}