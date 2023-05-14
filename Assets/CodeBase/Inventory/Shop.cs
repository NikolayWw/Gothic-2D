using CodeBase.Data.PlayerProgress.InventoryData;
using CodeBase.Logic;
using CodeBase.Services.GameFactory;
using CodeBase.Services.LogicFactory;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Items;
using CodeBase.UI.Windows.Inventory.UseInventoryButtons;
using System.Threading.Tasks;
using TMPro;

namespace CodeBase.Inventory
{
    public class Shop
    {
        private readonly InventorySlotsHandler _slotsHandler;
        private readonly InventorySlotsContainer _inventorySlotsContainer;
        private readonly UseItemInInventoryButton _button;
        private readonly IGameFactory _gameFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly InventorySlotsContainer _shopInventorySlotsContainer;

        private Slot _activeSlot;

        public Shop(ILogicFactoryService logicFactory, InventorySlotsContainer shopInventorySlotsContainer, InventorySlotsContainer playerInventorySlotsContainer, UseItemInInventoryButtonsWindow useButtonsWindow, IGameFactory gameFactory, IStaticDataService staticDataService)
        {
            _slotsHandler = logicFactory.InventorySlotsHandler;
            _inventorySlotsContainer = playerInventorySlotsContainer;
            _shopInventorySlotsContainer = shopInventorySlotsContainer;
            _button = useButtonsWindow.SellAndBuyButton;
            _gameFactory = gameFactory;
            _staticDataService = staticDataService;

            _inventorySlotsContainer.OnChangeValueSlot += UpdateButton;
            _shopInventorySlotsContainer.OnChangeValueSlot += UpdateButton;
        }

        public void Clean()
        {
            _inventorySlotsContainer.OnChangeValueSlot -= UpdateButton;
            _shopInventorySlotsContainer.OnChangeValueSlot -= UpdateButton;
            RemoveListeners();
        }

        private void UpdateButton()
        {
            if (_slotsHandler.IsFrameEnableAndItemPresent(_shopInventorySlotsContainer, out _activeSlot))
                InitButton(true);
            else if (_slotsHandler.IsFrameEnableAndItemPresent(_inventorySlotsContainer, out _activeSlot))
                InitButton(false);
            else
                _button.SetShow(false);
        }

        private void InitButton(bool isShop)
        {
            RemoveListeners();
            _button.SetShow(true);
            _button.Button.interactable = true;

            if (isShop)
            {
                int gold = _slotsHandler.CalculateAmount(ItemId.Gold, _inventorySlotsContainer);
                _button.Button.interactable = gold >= GetItem(_activeSlot.ItemId).PurchasePrice && _activeSlot.IsEquip == false;
                _button.Text.text = "Buy";
                _button.Button.onClick.AddListener(Buy);
            }
            else
            {
                _button.Button.interactable = _activeSlot.ItemId != ItemId.Gold && _activeSlot.IsEquip == false;
                _button.Button.GetComponentInChildren<TMP_Text>().text = "Sell";
                _button.Button.onClick.AddListener(Sell);
            }
        }

        private async void Sell()
        {
            int amount = 1;
            if (_slotsHandler.Add(_activeSlot.ItemId, ref amount, _shopInventorySlotsContainer))
            {
                int sell = GetItem(_activeSlot.ItemId).SalePrice;
                if (_slotsHandler.Add(ItemId.Gold, ref sell, _inventorySlotsContainer) == false)
                    await CreateItemPiece(ItemId.Gold, sell);
            }
            else
            {
                await DropItem(amount);
            }

            _activeSlot.DecrementOne();
        }

        private async void Buy()
        {
            int price = GetItem(_activeSlot.ItemId).PurchasePrice;
            _slotsHandler.DecrementAmount(ItemId.Gold, ref price, _inventorySlotsContainer);

            var amount = 1;
            if (_slotsHandler.Add(_activeSlot.ItemId, ref amount, _inventorySlotsContainer) == false)
                await DropItem(amount);

            _activeSlot.DecrementOne();
        }

        private async Task DropItem(int amount)
        {
            await CreateItemPiece(_activeSlot.ItemId, amount);
        }

        private void RemoveListeners()
        {
            _button.Button.onClick.RemoveListener(Buy);
            _button.Button.onClick.RemoveListener(Sell);
        }

        private BaseItem GetItem(ItemId id) =>
             _staticDataService.ForItem(id);

        private async Task CreateItemPiece(ItemId id, int amount)
        {
            var itemPiece = await _gameFactory.CreateItemPiece(id, amount, _gameFactory.Player.transform.position);
            var uniqueId = itemPiece.GetComponent<UniqueId>();
            uniqueId.GenerateId();
            itemPiece.Initialize(uniqueId.Id);
        }
    }
}