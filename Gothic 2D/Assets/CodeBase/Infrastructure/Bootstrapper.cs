using CodeBase.Infrastructure.Logic;
using CodeBase.Infrastructure.States;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class Bootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private async void Awake()
        {
            LoadCurtain loadCurtain = new LoadCurtain(this);
            loadCurtain.Show();

            Game game = new Game();
            await game.InitGameStateMachine(loadCurtain, this);
            game.GameStateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }
    }
}