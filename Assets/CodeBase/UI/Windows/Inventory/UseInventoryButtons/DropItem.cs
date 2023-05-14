#region

using CodeBase.Data.PlayerProgress.InventoryData;
using CodeBase.Inventory;
using CodeBase.Services.LogicFactory;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

#endregion

namespace CodeBase.UI.Windows.Inventory.UseInventoryButtons
{
    public class DropItem : MonoBehaviour
    {
        [SerializeField] private UseItemInInventoryButton _dropButton;

        private InventorySlotsHandler _slotsHandler;
        private InventorySlotsContainer _inventorySlotsContainer;
        private Slot _currentSlot;

        public void Construct(IPersistentProgressService persistentProgressService, ILogicFactoryService logicFactory)
        {
            _inventorySlotsContainer = persistentProgressService.PlayerProgress.PlayerData.SlotsContainer;
            _slotsHandler = logicFactory.InventorySlotsHandler;

            _inventorySlotsContainer.OnChangeValueSlot += UpdateButton;
            _dropButton.Button.onClick.AddListener(Drop);
        }

        private void OnDestroy() =>
            _inventorySlotsContainer.OnChangeValueSlot -= UpdateButton;

        private void UpdateButton()
        {
            _dropButton.SetShow(_slotsHandler.IsFrameEnableAndItemPresent(_inventorySlotsContainer, out _currentSlot));
            _dropButton.Button.interactable = _currentSlot?.IsEquip == false;
        }

        private async void Drop() =>
         await _slotsHandler.DropItem(_currentSlot);
    }
}