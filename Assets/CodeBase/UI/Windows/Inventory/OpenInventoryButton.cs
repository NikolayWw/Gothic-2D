#region

using CodeBase.Inventory;
using CodeBase.Services.LogicFactory;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace CodeBase.UI.Windows.Inventory
{
    public class OpenInventoryButton : BaseWindow
    {
        [SerializeField] private Button _openInventoryButton;
        private InventoryDragLogic _inventoryDragLogic;

        public void Construct(ILogicFactoryService logicFactory)
        {
            _inventoryDragLogic = logicFactory.InventoryDragLogic;
            _openInventoryButton.onClick.AddListener(OpenAndCloseInventory);
        }

        private async void OpenAndCloseInventory()
        {
            if (_inventoryDragLogic.IsInventoryOpen)
                _inventoryDragLogic.CloseInventoryWindow();
            else
                await _inventoryDragLogic.OpenInventory();
        }
    }
}