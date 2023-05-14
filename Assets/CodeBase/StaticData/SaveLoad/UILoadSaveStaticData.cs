using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.StaticData.SaveLoad
{
    [CreateAssetMenu(fileName = "New SaveLoadStaticData", menuName = "Static Data/UI Save Info Data", order = 0)]
    public class UILoadSaveStaticData : ScriptableObject
    {
        [field: SerializeField] public AssetReferenceGameObject SaveInfoDescriptionReference { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject LoadSaveButtonReference { get; private set; }
        [field: SerializeField] public int SavesCount { get; private set; } = 1;

        private void OnValidate()
        {
            if (SavesCount < 1) SavesCount = 1;
        }
    }
}