#region

using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

#endregion

namespace CodeBase.StaticData.Npc
{
    [Serializable]
    public class NpcConfig
    {
        [SerializeField] private string _name = string.Empty;
        [field: SerializeField] public NpcId Id { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject TemplateReference { get; private set; }
        [field: SerializeField] public int MaxHealth { get; private set; } = 1;
        [field: SerializeField] public int CurrentHealth { get; private set; } = 1;
        [field: SerializeField] public float PatrolSpeed { get; private set; } = 1;
        [field: SerializeField] public float WalkingSpeed { get; private set; } = 1;
        [field: SerializeField] public float FollowSpeed { get; private set; } = 1;

        public void OnValidate() =>
            _name = Id.ToString();
    }
}