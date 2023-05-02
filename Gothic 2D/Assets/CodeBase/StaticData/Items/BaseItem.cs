using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.StaticData.Items
{
    public abstract class BaseItem
    {
        [field: SerializeField] public string InspectorName { get; private set; }
        [field: SerializeField] public ItemId Id { get; private set; }
        [field: SerializeField] public bool IsMaxCountOne { get; private set; }
        [field: SerializeField] public int MaxCount { get; private set; } = 1;
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject PrefabPieceReference { get; private set; }
        [field: SerializeField] public int PurchasePrice { get; private set; } = 1;
        [field: SerializeField] public int SalePrice { get; private set; } = 1;

        public void OnValidate()
        {
            InspectorName = Id.ToString();

            if (MaxCount < 1) MaxCount = 1;
            if (PurchasePrice < 1) PurchasePrice = 1;
            if (SalePrice < 0) SalePrice = 0;
        }
    }
}