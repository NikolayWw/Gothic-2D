using CodeBase.StaticData.Items;
using System;
using UnityEngine;

namespace CodeBase.Data.PlayerProgress.InventoryData
{
    [Serializable]
    public class Slot
    {
        public Action OnChangeValueSlot;
        public Action<Slot> OnChangeFrameSelect;
        public Action<Slot> OnChangeEquip;
        public bool IsFrameSelected { get; private set; }
        [field: SerializeField] public bool IsEquip { get; private set; }
        [field: SerializeField] public ItemId ItemId { get; private set; }
        [field: SerializeField] public int Amount { get; private set; }
        [field: SerializeField] public bool IsEmpty { get; private set; } = true;

        public void SetFrameSelected(bool isSelected)
        {
            IsFrameSelected = isSelected;
            SendChangeFrameSelect();
            SendChangeValue();
        }

        public void SetEquip(bool isEquip)
        {
            IsEquip = isEquip;
            OnChangeEquip?.Invoke(this);
            SendChangeValue();
        }

        public void SetItem(ItemId itemId, int amount)
        {
            if (amount == 0 || itemId == ItemId.None)
            {
                Clear();
                return;
            }

            ItemId = itemId;
            Amount = amount;
            IsEmpty = false;
            SendChangeValue();
        }

        public void DecrementOne()
        {
            DecrementAmount(1);
        }

        public void DecrementAmount(int value)
        {
            Amount -= value;
            if (Amount < 0)
                Debug.LogError("Amount is less then null");

            if (Amount == 0)
                Clear();
            else
                SendChangeValue();
        }

        public void Clear()
        {
            Amount = 0;
            ItemId = ItemId.None;
            IsEmpty = true;
            SendChangeValue();
        }

        private void SendChangeFrameSelect()
        {
            OnChangeFrameSelect?.Invoke(this);
        }

        private void SendChangeValue()
        {
            OnChangeValueSlot?.Invoke();
        }
    }
}