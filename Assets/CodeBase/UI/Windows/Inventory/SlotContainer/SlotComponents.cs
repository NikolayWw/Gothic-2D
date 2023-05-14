using UnityEngine;

namespace CodeBase.UI.Windows.Inventory.SlotContainer
{
    public class SlotComponents : MonoBehaviour
    {
        [field: SerializeField] public UISlotUpdate UISlotUpdate { get; private set; }
        [field: SerializeField] public UISlotClickReporter UISlotClickReporter { get; private set; }
    }
}