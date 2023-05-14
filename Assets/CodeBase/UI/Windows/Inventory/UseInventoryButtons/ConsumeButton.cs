using CodeBase.Data.PlayerProgress.InventoryData;
using CodeBase.Inventory;
using CodeBase.Player.UseItems;
using CodeBase.Services.LogicFactory;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.UI.Windows.Inventory.UseInventoryButtons
{
    public class ConsumeButton : MonoBehaviour
    {
        [SerializeField] private UseItemInInventoryButtonsWindow _buttonsWindow;
        [SerializeField] private UseItemInInventoryButton _consumeButton;
        private ConsumeItems _consumeItems;
        private InventorySlotsContainer _playerSlots;
        private InventorySlotsHandler _slotsHandler;
        private Slot _currentSlot;

        public void Construct(IPersistentProgressService persistentProgressService, ILogicFactoryService logicFactory, ConsumeItems consumeItems)
        {
            _playerSlots = persistentProgressService.PlayerProgress.PlayerData.SlotsContainer;
            _slotsHandler = logicFactory.InventorySlotsHandler;
            _consumeItems = consumeItems;
            _playerSlots.OnChangeValueSlot += UpdateButton;
        }

        private void OnDestroy()
        {
            _consumeButton.Button.onClick.RemoveListener(Consume);
            _playerSlots.OnChangeValueSlot -= UpdateButton;
        }

        private void UpdateButton()
        {
            _consumeButton.Button.onClick.RemoveListener(Consume);
            if (CanConsume())
            {
                _consumeButton.Button.onClick.AddListener(Consume);
                _consumeButton.Text.text = "Consume";
                _buttonsWindow.SetConsume(true);
            }
            else
            {
                _buttonsWindow.SetConsume(false);
            }
        }

        private void Consume() =>
            _consumeItems?.Consume(_currentSlot);

        private bool CanConsume()
        {
            return _slotsHandler.IsFrameEnableAndItemPresent(_playerSlots, out _currentSlot)
                   && _consumeItems.CanConsume(_currentSlot.ItemId);
        }
    }
}