#region

using UnityEngine;
using UnityEngine.EventSystems;

#endregion

namespace CodeBase.UI.Windows.Inventory.SlotContainer
{
    public class UISlotClickReporter : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private UIInventorySlotsContainer _slotsContainer;
        private int _index;

        public void Initialize(int index, UIInventorySlotsContainer slotsContainer)
        {
            _index = index;
            _slotsContainer = slotsContainer;
        }

        public void OnPointerDown(PointerEventData eventData) =>
            _slotsContainer.OnDownClick?.Invoke(_slotsContainer.InventorySlotsContainer, _slotsContainer.InventorySlotsContainer.Slots[_index]);

        public void OnPointerUp(PointerEventData eventData) =>
            _slotsContainer.OnUpClick?.Invoke(_slotsContainer.InventorySlotsContainer, _slotsContainer.InventorySlotsContainer.Slots[_index]);
    }
}