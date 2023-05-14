using CodeBase.Data;
using CodeBase.Infrastructure.States;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.LoadSaveMenu
{
    public class SaveDescriptionPanel : BaseWindow
    {
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Button _clearProgressButton;

        private ISaveLoadService _saveLoadService;
        private int _progressIndex;
        private IGameStateMachine _gameStateMachine;
        private IPersistentProgressService _persistentProgressService;

        public void Construct(IGameStateMachine gameStateMachine, ISaveLoadService saveLoadService, IPersistentProgressService persistentProgressService)
        {
            _gameStateMachine = gameStateMachine;
            _saveLoadService = saveLoadService;
            _persistentProgressService = persistentProgressService;

            _saveLoadService.OnLoadAllProgress += RefreshButtons;
            _clearProgressButton.onClick.AddListener(ClearProgress);
            _saveButton.onClick.AddListener(Save);
            _loadButton.onClick.AddListener(LoadProgress);
        }

        public void Initialize(int progressIndex)
        {
            _progressIndex = progressIndex;
            RefreshButtons();
        }

        private void OnDestroy()
        {
            _saveLoadService.OnLoadAllProgress -= RefreshButtons;
        }

        private void Save()
        {
            _saveLoadService.SaveProgress(_progressIndex);
            _saveLoadService.LoadAllProgress();
        }

        private void LoadProgress()
        {
            _persistentProgressService.PlayerProgress = _saveLoadService.PlayerProgresses[_progressIndex];
            _gameStateMachine.Enter<LoadLevelState, string>(_persistentProgressService.PlayerProgress.WorldData.PositionOnLevel.Level);
        }

        private void ClearProgress() =>
            _saveLoadService.ClearProgress(_progressIndex);

        private void RefreshButtons()
        {
            _clearProgressButton.interactable = _saveLoadService.PlayerProgresses[_progressIndex] != null;
            _loadButton.interactable = _saveLoadService.PlayerProgresses[_progressIndex] != null;
            _saveButton.interactable = IsMainMenuSceneCurrent() == false;
        }

        private static bool IsMainMenuSceneCurrent() =>
            SceneManager.GetActiveScene().name == GameConstants.MainMenuScene;
    }
}