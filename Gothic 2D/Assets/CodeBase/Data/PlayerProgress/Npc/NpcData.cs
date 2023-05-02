using System;
using System.Collections.Generic;

namespace CodeBase.Data.PlayerProgress.Npc
{
    [Serializable]
    public class NpcData
    {
        public NpcCharacteristics NpcCharacteristics;
        public List<string> DialogNpcKnows;
    }
}