using CodeBase.Data.PlayerProgress;
using CodeBase.Data.PlayerProgress.InventoryData;
using CodeBase.Services.GameFactory;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Items;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Player.UseItems
{
    public class EquipItems : ISaveLoad
    {
        private readonly Transform _weaponAnchor;
        private readonly IGameFactory _gameFactory;
        private readonly IStaticDataService _dataService;

        public EquipItems(Transform weaponAnchor, IGameFactory gameFactory, IStaticDataService dataService, IPersistentProgressService persistentProgressService)
        {
            _weaponAnchor = weaponAnchor;
            _gameFactory = gameFactory;
            _dataService = dataService;
        }

        public async void LoadProgress(PlayerProgress progress)
        {
            var slotsContainer = progress.PlayerData.SlotsContainer;
            if (slotsContainer.CurrentEquipSlot?.IsEquip == true)
            {
                await _gameFactory.CreateMeleeWeaponInHand((GetItem(slotsContainer.CurrentEquipSlot.ItemId) as MeleeWeaponConfig)?.PrefabReferenceInHand, _weaponAnchor);
            }
        }

        public void UpdateProgress(PlayerProgress progress)
        { }

        public bool IsEquipItem(ItemId id)
        {
            switch (GetItem(id))
            {
                case MeleeWeaponConfig _:
                    return true;
            }

            return false;
        }

        public async Task EquipOrTakeOff(InventorySlotsContainer inventory, Slot slot)
        {
            if (slot == inventory.CurrentEquipSlot)
            {
                if (slot.IsEquip)
                    TakeOff(inventory, slot);
                else
                    await Equip(slot);
            }
            else
            {
                if (inventory.CurrentEquipSlot?.IsEquip == true)
                    TakeOff(inventory, slot);
                await Equip(slot);
            }
        }

        private async Task Equip(Slot slot)
        {
            switch (GetItem(slot.ItemId))
            {
                case MeleeWeaponConfig weaponConfig:
                    await _gameFactory.CreateMeleeWeaponInHand(weaponConfig.PrefabReferenceInHand, _weaponAnchor);
                    slot.SetEquip(true);
                    break;
            }
        }

        private void TakeOff(InventorySlotsContainer inventory, Slot slot)
        {
            switch (GetItem(slot.ItemId))
            {
                case MeleeWeaponConfig _:
                    _gameFactory.ClearMeleeWeaponInHand();
                    inventory.CurrentEquipSlot?.SetEquip(false);
                    break;
            }
        }

        private BaseItem GetItem(ItemId id) =>
            _dataService.ForItem(id);
    }
}