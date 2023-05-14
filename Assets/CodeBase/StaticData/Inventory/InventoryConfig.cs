#region

using UnityEngine;
using UnityEngine.AddressableAssets;

#endregion

namespace CodeBase.StaticData.Inventory
{
    [CreateAssetMenu(fileName = "New InventoryConfig", menuName = "Static Data/Inventory/Inventory Config", order = 0)]
    public class InventoryConfig : ScriptableObject
    {
        [field: SerializeField] public int SlotCount { get; private set; } = 1;
        [field: SerializeField] public int SmallChestSlotCount { get; private set; } = 1;
        [field: SerializeField] public AssetReferenceGameObject SlotComponents { get; private set; }

        private void OnValidate()
        {
            if (SlotCount < 1) SlotCount = 1;
            if (SmallChestSlotCount < 1) SmallChestSlotCount = 1;
        }
    }
}