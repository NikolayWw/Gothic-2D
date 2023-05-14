#region

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace CodeBase.UI.Windows.Inventory.UseInventoryButtons
{
    [Serializable]
    public class UseItemInInventoryButton
    {
        [field: SerializeField] public Button Button { get; private set; }
        [field: SerializeField] public TMP_Text Text { get; private set; }
        public Action OnChangeState;

        public bool IsEnable => Button.gameObject.activeInHierarchy;

        public void SetShow(bool isShow)
        {
            Button.gameObject.SetActive(isShow);
            OnChangeState?.Invoke();
        }
    }
}