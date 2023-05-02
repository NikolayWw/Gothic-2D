using System;
using UnityEngine;

namespace CodeBase.Data.PlayerProgress.InventoryData
{
    [Serializable]
    public class InventorySlotsContainer
    {
        public Action OnChangeValueSlot;
        public Slot CurrentFrameSlot { get; private set; }
        public Slot CurrentEquipSlot { get; private set; }
        [field: SerializeField] public InventoryType InventoryType { get; private set; }
        [SerializeField] private Slot[] _slots;
        public Slot[] Slots { get => _slots; private set => _slots = value; }

        public InventorySlotsContainer(int cellCount, InventoryType type)
        {
            InventoryType = type;
            CreateSlots(cellCount);
        }

        public void InitSlots()
        {
            foreach (Slot slot in Slots)
            {
                slot.OnChangeFrameSelect += SetCurrentChangeFrameSelect;
                slot.OnChangeEquip += SetCurrentEquip;
                slot.OnChangeValueSlot += SendChangeValue;
                if (slot.IsEquip)
                    CurrentEquipSlot = slot;
            }
        }

        private void CreateSlots(int cellCount)
        {
            Slots = new Slot[cellCount];
            for (var i = 0; i < cellCount; i++)
                Slots[i] = new Slot();
        }

        private void SetCurrentEquip(Slot slot)
        {
            CurrentEquipSlot = slot;
        }

        private void SetCurrentChangeFrameSelect(Slot slot)
        {
            CurrentFrameSlot = slot;
        }

        private void SendChangeValue()
        {
            OnChangeValueSlot?.Invoke();
        }
    }
}