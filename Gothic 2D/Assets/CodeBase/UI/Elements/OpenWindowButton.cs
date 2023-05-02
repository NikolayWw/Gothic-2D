using CodeBase.Infrastructure.Logic;
using CodeBase.Services.LogicFactory;
using CodeBase.UI.Services.Window;
using CodeBase.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class OpenWindowButton : BaseWindow
    {
        [SerializeField] private WindowId _openWindowId;
        [SerializeField] private Button _openButton;

        private IWindowService _windowService;
        private CloseGameWindows _closeGameWindows;

        public void Construct(IWindowService windowService, ILogicFactoryService logicFactory)
        {
            _windowService = windowService;
            _closeGameWindows = logicFactory.CloseGameWindows;

            _openButton.onClick.AddListener(DisableWindows);
            _openButton.onClick.AddListener(Open);
        }

        private void Open()
        {
            if (_windowService.IsWindowOpen(_openWindowId))
                _windowService.Close(_openWindowId);
            else
                _windowService.Open(_openWindowId);
        }

        private void DisableWindows() =>
            _closeGameWindows.Close(_openWindowId);
    }
}