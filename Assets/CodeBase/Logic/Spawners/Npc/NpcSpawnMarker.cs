using CodeBase.StaticData.Npc;
using UnityEngine;

namespace CodeBase.Logic.Spawners.Npc
{
    public class NpcSpawnMarker : MonoBehaviour
    {
        [field: SerializeField] public NpcId Id { get; private set; }

        private void OnValidate()
        {
            if (IsPrefab() == false)
                gameObject.name = $"{Id}_NpcSpawnMarker";
        }

        private bool IsPrefab() =>
            gameObject.scene.rootCount == 0;
    }
}