using CodeBase.UI.Services.Window;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.LoadSaveMenu
{
    public class LoadSaveButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _buttonNameText;
        [SerializeField] private Button _loadSaveButton;
        private IWindowService _windowService;
        private int _saveIndex;

        public void Construct(IWindowService windowService)
        {
            _windowService = windowService;
        }

        public void Initialize(int progressIndex, in string buttonName)
        {
            _saveIndex = progressIndex;
            _buttonNameText.text = buttonName;
            _loadSaveButton.onClick.AddListener(OpenSaveDescription);
        }

        public void ChangeButtonName(in string buttonName) =>
            _buttonNameText.text = buttonName;

        private async void OpenSaveDescription()
        {
            var window = await _windowService.GetOrOpenWindow<SaveDescriptionPanel>(WindowId.SaveDescriptionPanel);
            window.Initialize(_saveIndex);
        }
    }
}