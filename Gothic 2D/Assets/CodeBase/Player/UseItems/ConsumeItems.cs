using CodeBase.Data.PlayerProgress.InventoryData;
using CodeBase.Data.PlayerProgress.Player;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Items;

namespace CodeBase.Player.UseItems
{
    public class ConsumeItems
    {
        private readonly IStaticDataService _dataService;
        private readonly PlayerCharacteristics _characteristics;

        public ConsumeItems(IStaticDataService dataService, IPersistentProgressService persistentProgressService)
        {
            _characteristics = persistentProgressService.PlayerProgress.PlayerData.PlayerCharacteristics;
            _dataService = dataService;
        }

        public bool CanConsume(ItemId id)
        {
            switch (GetItem(id))
            {
                case FoodConfig _:
                    return true;
            }
            return false;
        }

        public void Consume(Slot slot)
        {
            if (slot.Amount <= 0)
                return;

            switch (GetItem(slot.ItemId))
            {
                case FoodConfig foodConfig:
                    _characteristics.ChangeCharacteristics(foodConfig.MaxHealth, foodConfig.CurrentHealth, foodConfig.Strength);
                    slot.DecrementOne();
                    break;
            }
        }

        private BaseItem GetItem(ItemId id) =>
            _dataService.ForItem(id);
    }
}