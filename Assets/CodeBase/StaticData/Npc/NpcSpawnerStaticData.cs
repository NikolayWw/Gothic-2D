#region

using System;
using UnityEngine;

#endregion

namespace CodeBase.StaticData.Npc
{
    [Serializable]
    public class NpcSpawnerStaticData
    {
        [field: SerializeField] public string InspectorName { get; private set; }
        [field: SerializeField] public NpcId Id { get; private set; }
        [field: SerializeField] public Vector2 Position { get; private set; }

        public NpcSpawnerStaticData(NpcId id, Vector2 at)
        {
            Id = id;
            ReName();
            Position = at;
        }

        public void OnValidate() =>
            ReName();

        private void ReName() =>
            InspectorName = Id.ToString();
    }
}