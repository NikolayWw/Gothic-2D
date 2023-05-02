using CodeBase.Logic.Spawners.Items;
using CodeBase.Logic.Spawners.Npc;
using CodeBase.StaticData.Items;
using CodeBase.StaticData.Level;
using CodeBase.StaticData.Npc;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticDataEditor : UnityEditor.Editor
    {
        private const string PlayerInitialPointTag = "PlayerInitialPoint";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var data = (LevelStaticData)target;

            if (GUILayout.Button("Collect"))
            {
                GameObject initialPoint = GameObject.FindWithTag(PlayerInitialPointTag);
                data.PlayerInitialPoint = initialPoint.transform.position;

                data.NpcSpawnerStaticData = FindObjectsOfType<NpcSpawnMarker>().Select(x => new NpcSpawnerStaticData(x.Id, x.transform.position)).ToList();
                data.ItemSpawnerStaticDatas = FindObjectsOfType<ItemSpawnMarker>().Select(x => new ItemSpawnerStaticData(x.UniqueId.Id, x.Id, x.Amount, x.transform.position)).ToList();
                data.SceneKey = SceneManager.GetActiveScene().name;
                EditorUtility.SetDirty(data);
            }
        }
    }
}