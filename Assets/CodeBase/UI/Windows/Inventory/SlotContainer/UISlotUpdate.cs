#region

using CodeBase.Data.PlayerProgress.InventoryData;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace CodeBase.UI.Windows.Inventory.SlotContainer
{
    public class UISlotUpdate : MonoBehaviour
    {
        [SerializeField] private Image _equipImage;
        [SerializeField] private Image _selectImage;
        [SerializeField] private Image _iconImage;
        [SerializeField] private TMP_Text _amountText;

        [SerializeField] private Sprite _emptySprite;
        [SerializeField] private Sprite _equipSprite;
        [SerializeField] private Sprite _selectedSprite;
        private Slot _slot;
        private IStaticDataService _dataService;

        public void Construct(IStaticDataService dataService)
        {
            _dataService = dataService;
        }

        public void Initialize(Slot slot)
        {
            _slot = slot;
            _slot.OnChangeValueSlot += Refresh;
            Refresh();
        }

        private void OnDestroy()
        {
            _slot.OnChangeValueSlot -= Refresh;
        }

        private void Refresh()
        {
            _iconImage.sprite = _slot.IsEmpty == false ? GetItem(_slot.ItemId).Icon : _emptySprite;
            _equipImage.sprite = _slot.IsEquip ? _equipSprite : _emptySprite;
            _selectImage.sprite = _slot.IsFrameSelected ? _selectedSprite : _emptySprite;

            _amountText.text =
                _slot.IsEmpty ? string.Empty :
                GetItem(_slot.ItemId).IsMaxCountOne && _slot.Amount == 1 ? string.Empty :
                _slot.Amount.ToString();
        }

        private BaseItem GetItem(ItemId id) =>
            _dataService.ForItem(id);
    }
}