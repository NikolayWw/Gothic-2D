using CodeBase.StaticData.Items;
using CodeBase.StaticData.Npc;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.StaticData.Level
{
    [CreateAssetMenu(fileName = "New Level Data", menuName = "Static Data/Level/LevelData")]
    public class LevelStaticData : ScriptableObject
    {
        [field: SerializeField] public string SceneKey { get; set; }
        [field: SerializeField] public Vector2 PlayerInitialPoint { get; set; }
        [field: SerializeField] public List<NpcSpawnerStaticData> NpcSpawnerStaticData { get; set; }
        [field: SerializeField] public List<ItemSpawnerStaticData> ItemSpawnerStaticDatas { get; set; }

        private void OnValidate()
        {
            NpcSpawnerStaticData.ForEach(x => x.OnValidate());
        }
    }
}