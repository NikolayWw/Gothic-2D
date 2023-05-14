using CodeBase.StaticData.Items;
using UnityEngine;

namespace CodeBase.Logic.Spawners.Items
{
    public class ItemSpawnMarker : MonoBehaviour
    {
        [field: SerializeField] public UniqueId UniqueId { get; private set; }
        [field: SerializeField] public ItemId Id { get; private set; }
        [field: SerializeField] public int Amount { get; private set; } = 1;

        private void OnValidate()
        {
            if (Amount < 1) Amount = 1;
            if (IsPrefab() == false)
                gameObject.name = $"{Id}_ItemSpawnMarker";
        }

        private bool IsPrefab() =>
            gameObject.scene.rootCount == 0;
    }
}