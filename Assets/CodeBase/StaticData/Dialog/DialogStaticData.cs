using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.StaticData.Dialog
{
    [CreateAssetMenu(fileName = "New DialogStaticData", menuName = "Static Data/Dialog Prefabs Container", order = 0)]
    public class DialogStaticData : ScriptableObject
    {
        [field: SerializeField] public AssetReferenceGameObject DialogWindowReference { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject DialogButtonReference { get; private set; }
        [field: SerializeField] public float DialogueTimeWithoutSound { get; private set; } = 0.1f;
        [field: SerializeField] public List<DialogConfig> DialogConfigs { get; private set; }

        private void OnValidate()
        {
            if (DialogueTimeWithoutSound < 0.1f)
                DialogueTimeWithoutSound = 0.1f;

            DialogConfigs.ForEach(x => x.OnValidate());
        }
    }
}