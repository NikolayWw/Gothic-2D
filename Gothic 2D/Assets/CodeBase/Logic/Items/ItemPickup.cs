#region

using CodeBase.Data.PlayerProgress.InventoryData;
using CodeBase.Infrastructure.Logic;
using CodeBase.Inventory;
using CodeBase.Services.LogicFactory;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

#endregion

namespace CodeBase.Logic.Items
{
    public class ItemPickup : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private PlayerTriggerReporter _playerTriggerReporter;
        [SerializeField] private ItemPiece _itemPiece;

        private BlockPickupAfterCreate _blockPickup;

        private InventorySlotsContainer _inventorySlotsContainer;
        private InventorySlotsHandler _slotsHandler;

        public void Construct(IPersistentProgressService persistentProgressService, ILogicFactoryService logicFactory)
        {
            _inventorySlotsContainer = persistentProgressService.PlayerProgress.PlayerData.SlotsContainer;
            _slotsHandler = logicFactory.InventorySlotsHandler;
            _blockPickup = new BlockPickupAfterCreate(this);

            _playerTriggerReporter.OnTriggeredEnter += TryPickup;
            _blockPickup.StartBlockTimer();
        }

        private void TryPickup()
        {
            if (_blockPickup.IsBlocked)
            {
                _playerTriggerReporter.OnTriggeredEnter -= TryPickup;
                _playerTriggerReporter.OnTriggeredEnter += Pickup;
            }
            else
            {
                Pickup();
            }
        }

        private void Pickup()
        {
            int amount = _itemPiece.Amount;
            bool inventoryIsFull = _slotsHandler.Add(_itemPiece.Id, ref amount, _inventorySlotsContainer);
            if (inventoryIsFull)
            {
                _playerTriggerReporter.OnTriggeredEnter -= Pickup;
                _itemPiece.Picked();
            }
            else
            {
                _itemPiece.SetAmount(amount);
            }
        }
    }
}