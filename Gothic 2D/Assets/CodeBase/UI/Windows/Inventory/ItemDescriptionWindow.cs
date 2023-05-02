#region

using CodeBase.Data.PlayerProgress.InventoryData;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Items;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace CodeBase.UI.Windows.Inventory
{
    public class ItemDescriptionWindow : BaseWindow
    {
        [SerializeField] private Image _imageIcon;
        [SerializeField] private TMP_Text _itemNameText;
        [SerializeField] private TMP_Text _itemContextText;
        [SerializeField] private TMP_Text _priceText;
        private IStaticDataService _staticDataService;

        public void Construct(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void UpdateDescription(InventorySlotsContainer inventory, Slot slot, bool isShopOpen)
        {
            BaseItem item = GetItem(slot.ItemId);

            _imageIcon.sprite = item.Icon;
            _itemNameText.text = item.InspectorName;
            _priceText.text = $"Price: {CalculatePrice(inventory, slot, isShopOpen)}";

            switch (GetItem(slot.ItemId))
            {
                case MeleeWeaponConfig weapon:
                    _itemContextText.text = $"Scale Damage: {weapon.StrengthScale}";
                    break;

                case FoodConfig food:
                    var sb = new StringBuilder();

                    if (food.MaxHealth != 0) sb.AppendLine($"Increase Max Health: {food.MaxHealth}");
                    if (food.CurrentHealth != 0) sb.AppendLine($"Heal: {food.CurrentHealth}");
                    if (food.Strength != 0) sb.AppendLine($"Increase  Strength: {food.Strength}");

                    _itemContextText.text = sb.ToString();
                    break;

                default:
                    _itemContextText.text = string.Empty;
                    break;
            }
        }

        private int CalculatePrice(InventorySlotsContainer inventory, Slot slot, bool isShopOpen)
        {
            BaseItem item = GetItem(slot.ItemId);
            if (isShopOpen)
                return inventory.InventoryType == InventoryType.Shop
                    ? item.PurchasePrice
                    : item.SalePrice;

            return item.PurchasePrice;
        }

        private BaseItem GetItem(ItemId id) =>
            _staticDataService.ForItem(id);
    }
}