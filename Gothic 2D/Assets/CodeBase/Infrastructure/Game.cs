using System.Threading.Tasks;
using CodeBase.Infrastructure.Logic;
using CodeBase.Infrastructure.States;
using CodeBase.Services;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        public IGameStateMachine GameStateMachine { get; private set; }

        public async Task InitGameStateMachine(LoadCurtain loadCurtain, ICoroutineRunner coroutineRunner)
        {
            GameStateMachine = new GameStateMachine();
            await GameStateMachine.InitStateMachine(new SceneLoader(coroutineRunner), loadCurtain, AllServices.Container, coroutineRunner);
        }
    }
}