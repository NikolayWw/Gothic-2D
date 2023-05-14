using CodeBase.Data.PlayerProgress.InventoryData;
using CodeBase.Inventory;
using CodeBase.Player.UseItems;
using CodeBase.Services.LogicFactory;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.UI.Windows.Inventory.UseInventoryButtons
{
    public class EquipButton : MonoBehaviour
    {
        [SerializeField] private UseItemInInventoryButtonsWindow _buttonsWindow;
        [SerializeField] private UseItemInInventoryButton _equipButton;

        private InventorySlotsContainer _inventorySlots;
        private InventorySlotsHandler _slotsHandler;
        private EquipItems _equipItems;

        private Slot _currentSlot;

        public void Construct(IPersistentProgressService persistentProgressService, ILogicFactoryService logicFactory, EquipItems equipItems)
        {
            _slotsHandler = logicFactory.InventorySlotsHandler;
            _inventorySlots = persistentProgressService.PlayerProgress.PlayerData.SlotsContainer;
            _equipItems = equipItems;
            _inventorySlots.OnChangeValueSlot += UpdateButton;
        }

        private void OnDestroy()
        {
            _equipButton.Button.onClick.RemoveListener(Equip);
            _inventorySlots.OnChangeValueSlot -= UpdateButton;
        }

        private void UpdateButton()
        {
            _equipButton.Button.onClick.RemoveListener(Equip);
            if (IsItemPresentAndEquip())
            {
                _equipButton.Button.onClick.AddListener(Equip);
                _equipButton.Text.text = _currentSlot?.IsEquip == true ? "Take off" : "Quip";
                _buttonsWindow.SetEquip(true);
            }
            else
            {
                _buttonsWindow.SetEquip(false);
            }
        }

        private async void Equip() =>
           await _equipItems.EquipOrTakeOff(_inventorySlots, _currentSlot);

        private bool IsItemPresentAndEquip()
        {
            return _slotsHandler.IsFrameEnableAndItemPresent(_inventorySlots, out _currentSlot)
                   && _equipItems.IsEquipItem(_currentSlot.ItemId);
        }
    }
}