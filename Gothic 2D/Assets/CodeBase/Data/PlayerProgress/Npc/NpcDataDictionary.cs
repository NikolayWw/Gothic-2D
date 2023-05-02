using CodeBase.StaticData.Npc;
using System;

namespace CodeBase.Data.PlayerProgress.Npc
{
    [Serializable]
    public class NpcDataDictionary : SerializableDictionary<NpcId, NpcData>
    { }
}