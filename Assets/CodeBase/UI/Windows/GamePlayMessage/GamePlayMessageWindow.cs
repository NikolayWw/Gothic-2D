using CodeBase.Data.PlayerProgress.Player;
using CodeBase.Services.PersistentProgress;
using CodeBase.UI.Services.Factory;
using UnityEngine;

namespace CodeBase.UI.Windows.GamePlayMessage
{
    public class GamePlayMessageWindow : BaseWindow
    {
        [SerializeField] private Transform _itemPoint;
        [SerializeField] private Transform _experiancePoint;
        [SerializeField] private Transform _strengthPoint;
        private IUIFactory _uiFactory;
        private PlayerCharacteristics _characteristics;

        public void Construct(IUIFactory uiFactory, IPersistentProgressService persistentProgressService)
        {
            _uiFactory = uiFactory;
            _characteristics = persistentProgressService.PlayerProgress.PlayerData.PlayerCharacteristics;
            _characteristics.OnIncrementExperience += ShowExperience;
        }

        private void OnDestroy() =>
            _characteristics.OnIncrementExperience -= ShowExperience;

        public async void ShowMessage(GamePlayMessageId id, string message)
        {
            switch (id)
            {
                case GamePlayMessageId.None:
                    break;

                case GamePlayMessageId.Experience:
                    await _uiFactory.CreateGameplayMessage(message, _experiancePoint);
                    break;

                case GamePlayMessageId.Item:
                    await _uiFactory.CreateGameplayMessage(message, _itemPoint);
                    break;

                case GamePlayMessageId.Strength:
                    await _uiFactory.CreateGameplayMessage(message, _strengthPoint);
                    break;
            }
        }

        private void ShowExperience(int value) =>
            ShowMessage(GamePlayMessageId.Experience, $"Опыт + {value}");
    }
}