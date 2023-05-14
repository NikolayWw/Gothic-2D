using System;

namespace CodeBase.Data.PlayerProgress.Player.Quests
{
    [Serializable]
    public class PlayerQuestsDictionary : SerializableDictionary<QuestId, QuestState>
    { }
}