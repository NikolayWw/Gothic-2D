using CodeBase.StaticData.Npc;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Data.PlayerProgress
{
    [Serializable]
    public class KillData
    {
        [field: SerializeField] public List<NpcId> Npc { get; private set; } = new List<NpcId>();
        [field: SerializeField] public List<string> ItemSpawner { get; private set; } = new List<string>();
    }
}