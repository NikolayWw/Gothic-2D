using CodeBase.Infrastructure.States;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.MainMenu
{
    public class LoadNewGameButton : MonoBehaviour
    {
        [SerializeField] private Button _newGameButton;
        private ISaveLoadService _loadService;
        private IGameStateMachine _gameStateMachine;
        private IPersistentProgressService _persistentProgressService;

        public void Construct(ISaveLoadService loadService, IGameStateMachine gameStateMachine, IPersistentProgressService persistentProgressService)
        {
            _loadService = loadService;
            _gameStateMachine = gameStateMachine;
            _persistentProgressService = persistentProgressService;
            _newGameButton.onClick.AddListener(StartNewGame);
        }

        private void StartNewGame()
        {
            _persistentProgressService.PlayerProgress = _loadService.NewProgress();
            _gameStateMachine.Enter<LoadLevelState, string>(_persistentProgressService.PlayerProgress.WorldData.PositionOnLevel.Level);
        }
    }
}