using System;
using UnityEngine;

namespace CodeBase.Data.PlayerProgress.Player.Quests
{
    [Serializable]
    public class PlayerQuest
    {
        [field: SerializeField] public QuestState State { get; private set; }

        public PlayerQuest(QuestState state) =>
            State = state;

        public void ChangeState(QuestState state) =>
            State = state;
    }
}