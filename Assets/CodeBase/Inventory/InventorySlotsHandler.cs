using CodeBase.Data.PlayerProgress.InventoryData;
using CodeBase.Logic;
using CodeBase.Services.GameFactory;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Items;
using System.Linq;
using System.Threading.Tasks;

namespace CodeBase.Inventory
{
    public class InventorySlotsHandler
    {
        private readonly IStaticDataService _dataService;
        private readonly IGameFactory _gameFactory;

        public InventorySlotsHandler(IStaticDataService dataService, IGameFactory gameFactory)
        {
            _dataService = dataService;
            _gameFactory = gameFactory;
        }

        public bool IsFrameEnableAndItemPresent(InventorySlotsContainer inventorySlotsContainer, out Slot activeSlot)
        {
            bool value =
                inventorySlotsContainer?.CurrentFrameSlot != null &&
                inventorySlotsContainer.CurrentFrameSlot.IsFrameSelected &&
                inventorySlotsContainer.CurrentFrameSlot.IsEmpty == false;

            activeSlot = inventorySlotsContainer?.CurrentFrameSlot;
            return value;
        }

        public async Task AddOrDropItem(ItemId id, int amount, InventorySlotsContainer inventorySlotsContainer)
        {
            if (Add(id, ref amount, inventorySlotsContainer) == false)
                await CreateItemPiece(id, amount);
        }

        public async Task DropItem(Slot slot)
        {
            await CreateItemPiece(slot.ItemId, slot.Amount);
            slot.Clear();
        }

        public bool Add(ItemId id, ref int amount, InventorySlotsContainer inventorySlotsContainer)
        {
            //Add
            if (TryAddItem(inventorySlotsContainer, id, ref amount))
                return true;
            //Set
            if (TrySetItem(inventorySlotsContainer, id, ref amount))
                return true;

            return false;
        }

        public void ExchangeSlotData(Slot slotTo, Slot slotFrom)
        {
            BaseItem GetItem(ItemId itemId) => _dataService.ForItem(itemId);

            if (slotTo == slotFrom)
                return;

            if (slotTo.IsEmpty == false && GetItem(slotTo.ItemId).IsMaxCountOne == false && slotTo.ItemId == slotFrom.ItemId)
            {
                BaseItem item = GetItem(slotFrom.ItemId);
                CalculateStackUp(slotTo.Amount, slotFrom.Amount, item.MaxCount, out var resultTo, out var resultFrom);
                slotTo.SetItem(item.Id, resultTo);
                slotFrom.SetItem(item.Id, resultFrom);
            }
            else
            {
                BaseItem item = GetItem(slotTo.ItemId);
                int amount = slotTo.Amount;
                slotTo.SetItem(slotFrom.ItemId, slotFrom.Amount);
                slotFrom.SetItem(item.Id, amount);
            }
        }

        public bool DecrementAmount(ItemId id, ref int amount, InventorySlotsContainer inventorySlotsContainer)
        {
            bool decrementAllAmount = false;
            foreach (var slot in inventorySlotsContainer.Slots)
            {
                if (slot.ItemId == id)
                {
                    if (slot.Amount - amount >= 0)
                    {
                        slot.DecrementAmount(amount);
                        decrementAllAmount = true;
                        break;
                    }
                    amount -= slot.Amount;
                    slot.Clear();
                }
            }

            return decrementAllAmount;
        }

        public void DeselectSlots(InventorySlotsContainer inventorySlotsContainer)
        {
            foreach (var slot in inventorySlotsContainer.Slots.Where(slot => slot.IsFrameSelected))
                slot.SetFrameSelected(false);
        }

        public int CalculateAmount(ItemId id, InventorySlotsContainer inventorySlotsContainer)
        {
            var tempAmount = 0;
            foreach (var slot in inventorySlotsContainer.Slots.Where(slot => slot.IsEmpty == false && slot.ItemId == id))
            {
                tempAmount += slot.Amount;
            }

            return tempAmount;
        }

        private bool TryAddItem(InventorySlotsContainer inventorySlotsContainer, ItemId id, ref int amount)
        {
            BaseItem GetItem(ItemId itemId) => _dataService.ForItem(itemId);

            foreach (var slot in inventorySlotsContainer.Slots)
            {
                if (slot.IsEmpty == false && slot.ItemId == id)
                    if (GetItem(slot.ItemId).IsMaxCountOne == false && slot.Amount < GetItem(slot.ItemId).MaxCount)
                    {
                        var completelyStackUp = CalculateStackUp(amount, slot.Amount,
                            GetItem(slot.ItemId).MaxCount, out var resultTo, out var resultFrom);
                        if (completelyStackUp == false)
                        {
                            slot.SetItem(slot.ItemId, resultFrom);
                            amount = resultTo;
                        }
                        else
                        {
                            slot.SetItem(slot.ItemId, resultFrom);
                            return true;
                        }
                    }
            }

            return false;
        }

        private bool TrySetItem(InventorySlotsContainer inventorySlotsContainer, ItemId id, ref int amount)
        {
            BaseItem GetItem(ItemId itemId) => _dataService.ForItem(itemId);
            for (int i = 0; i < inventorySlotsContainer.Slots.Count(); i++)
                if (inventorySlotsContainer.Slots[i].IsEmpty)
                {
                    if (GetItem(id).MaxCount < amount)
                    {
                        amount -= GetItem(id).MaxCount;
                        inventorySlotsContainer.Slots[i].SetItem(id, GetItem(id).MaxCount);
                    }
                    else
                    {
                        inventorySlotsContainer.Slots[i].SetItem(id, amount);
                        return true;
                    }
                }

            return false;
        }

        private static bool CalculateStackUp(int toAmount, int fromAmount, int maxCount, out int resultTo, out int resultFrom)
        {
            bool completelyStackUp;
            if (toAmount + fromAmount <= maxCount)
            {
                resultFrom = toAmount + fromAmount;
                resultTo = 0;
                completelyStackUp = true;
            }
            else
            {
                resultTo = toAmount + fromAmount - maxCount;
                resultFrom = maxCount;
                completelyStackUp = false;
            }

            return completelyStackUp;
        }

        private async Task CreateItemPiece(ItemId id, int amount)
        {
            var itemPiece = await _gameFactory.CreateItemPiece(id, amount, _gameFactory.Player.transform.position);
            var uniqueId = itemPiece.GetComponent<UniqueId>();
            uniqueId.GenerateId();
            itemPiece.Initialize(uniqueId.Id);
        }
    }
}