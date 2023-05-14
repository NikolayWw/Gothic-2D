using CodeBase.Data;
using CodeBase.UI.Services.Window;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.LoadSaveMenu
{
    public class CloseSaveMenuButton : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        private IWindowService _windowService;

        public void Construct(IWindowService windowService)
        {
            _windowService = windowService;
            _closeButton.onClick.AddListener(OpenPreviousWindow);
        }

        private async void OpenPreviousWindow()
        {
            string sceneName = SceneManager.GetActiveScene().name;

            await _windowService.Open(ConditionToOpenMainMenu(in sceneName) ? WindowId.MainMenu : WindowId.GameMenu);
            _windowService.Close(WindowId.LoadSaveMenu);
        }

        private static bool ConditionToOpenMainMenu(in string sceneName) =>
            sceneName == GameConstants.MainMenuScene;
    }
}