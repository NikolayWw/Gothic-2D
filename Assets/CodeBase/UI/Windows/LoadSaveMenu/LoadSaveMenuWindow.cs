using CodeBase.Data;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Window;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.UI.Windows.LoadSaveMenu
{
    public class LoadSaveMenuWindow : BaseWindow
    {
        [SerializeField] private Transform _loadSaveButtonsAnhour;

        private IStaticDataService _dataService;
        private IUIFactory _uiFactory;
        private ISaveLoadService _saveLoadService;
        private IWindowService _windowService;
        private readonly List<LoadSaveButton> _buttons = new List<LoadSaveButton>();

        public void Construct(IStaticDataService dataService, ISaveLoadService saveLoadService, IUIFactory uiFactory, IWindowService windowService)
        {
            _dataService = dataService;
            _saveLoadService = saveLoadService;
            _uiFactory = uiFactory;
            _windowService = windowService;
            _saveLoadService.OnLoadAllProgress += ResetButtonsNames;
        }

        private async void Start()
        {
            for (int i = 0; i < _dataService.UILoadSaveStaticData.SavesCount; i++)
            {
                LoadSaveButton button = await _uiFactory.CreateLoadSaveButton(_windowService, _loadSaveButtonsAnhour);
                var buttonName = _saveLoadService.PlayerProgresses[i] != null ? _saveLoadService.PlayerProgresses[i].ProgressName : GameConstants.NullSaveName;
                button.Initialize(i, buttonName);
                _buttons.Add(button);
            }
        }

        private void OnDestroy()
        {
            _saveLoadService.OnLoadAllProgress -= ResetButtonsNames;
        }

        protected override void OnClose()
        {
            _windowService.Close(WindowId.SaveDescriptionPanel);
            base.OnClose();
        }

        private void ResetButtonsNames()
        {
            for (int i = 0; i < _saveLoadService.PlayerProgresses.Length; i++)
                if (_saveLoadService.PlayerProgresses[i] != null)
                    _buttons[i].ChangeButtonName(_saveLoadService.PlayerProgresses[i].ProgressName);
                else
                    _buttons[i].ChangeButtonName(GameConstants.NullSaveName);
        }
    }
}