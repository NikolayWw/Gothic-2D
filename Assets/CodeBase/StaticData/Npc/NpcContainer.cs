#region

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace CodeBase.StaticData.Npc
{
    [CreateAssetMenu(fileName = "New Npc Container", menuName = "Static Data/Npc Container", order = 0)]
    public class NpcContainer : ScriptableObject
    {
        [field: SerializeField] public List<NpcConfig> Configs { get; private set; }

        private void OnValidate() =>
            Configs.ForEach(x => x.OnValidate());
    }
}