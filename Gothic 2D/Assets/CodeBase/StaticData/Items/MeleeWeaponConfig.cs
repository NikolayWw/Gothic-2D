using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.StaticData.Items
{
    [Serializable]
    public class MeleeWeaponConfig : BaseItem
    {
        [field: SerializeField] public AssetReferenceGameObject PrefabReferenceInHand { get; private set; }
        [field: SerializeField] public float StrengthScale { get; private set; }
    }
}