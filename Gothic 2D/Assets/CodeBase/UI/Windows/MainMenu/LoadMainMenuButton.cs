using CodeBase.Infrastructure.States;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.MainMenu
{
    public class LoadMainMenuButton : MonoBehaviour
    {
        [SerializeField] private Button _loadMainMenuButton;
        private IGameStateMachine _gameStateMachine;

        public void Construct(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
            _loadMainMenuButton.onClick.AddListener(() => _gameStateMachine.Enter<LoadMainMenuState>());
        }
    }
}