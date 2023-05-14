using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.StaticData.GamePlayMessage
{
    [CreateAssetMenu(fileName = "New GameplayMessageStaticData", menuName = "Static Data/GameplayMessageStaticData", order = 0)]
    public class GameplayMessageStaticData : ScriptableObject
    {
        [field: SerializeField] public AssetReferenceGameObject MessageReference { get; private set; }
        [field: SerializeField] public float LifeTime { get; private set; }
    }
}