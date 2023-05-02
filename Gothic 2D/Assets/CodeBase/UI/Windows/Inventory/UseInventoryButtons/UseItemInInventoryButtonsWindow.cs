#region

using UnityEngine;

#endregion

namespace CodeBase.UI.Windows.Inventory.UseInventoryButtons
{
    public class UseItemInInventoryButtonsWindow : BaseWindow
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [field: SerializeField] public UseItemInInventoryButton UseButton { get; private set; }
        [field: SerializeField] public UseItemInInventoryButton DropButton { get; private set; }
        [field: SerializeField] public UseItemInInventoryButton SellAndBuyButton { get; private set; }

        private bool _isEquipEnable;
        private bool _isConsumeEnable;

        private void Awake()
        {
            HideThisWindow();
        }

        private void Start()
        {
            UseButton.OnChangeState += HideOrShowWindow;
            DropButton.OnChangeState += HideOrShowWindow;
            SellAndBuyButton.OnChangeState += HideOrShowWindow;
        }

        public void SetEquip(bool isEnable)
        {
            _isEquipEnable = isEnable;
            HideOrShowUseButton();
        }

        public void SetConsume(bool isEnable)
        {
            _isConsumeEnable = isEnable;
            HideOrShowUseButton();
        }

        private void HideOrShowUseButton()
        {
            bool activeState = _isConsumeEnable || _isEquipEnable;
            UseButton.SetShow(activeState);
            HideOrShowWindow();
        }

        private void HideOrShowWindow()
        {
            _canvasGroup.alpha =
                UseButton.IsEnable ? 1 :
                DropButton.Button.gameObject.activeInHierarchy ? 1 :
                SellAndBuyButton.Button.gameObject.activeInHierarchy ? 1 : 0;
        }

        private void HideThisWindow()
        {
            _canvasGroup.alpha = 0;
            UseButton.SetShow(false);
            DropButton.SetShow(false);
            SellAndBuyButton.SetShow(false);
        }
    }
}