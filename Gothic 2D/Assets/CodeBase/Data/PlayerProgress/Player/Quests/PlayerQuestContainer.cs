using System;
using UnityEngine;

namespace CodeBase.Data.PlayerProgress.Player.Quests
{
    [Serializable]
    public class PlayerQuestContainer
    {
        [SerializeField] private PlayerQuestsDictionary _questsDictionary;

        public Action OnChangeQuestState;
        public Action<QuestId, QuestState> OnChangeQuestStateSendInfo;

        public PlayerQuestContainer() =>
            _questsDictionary = new PlayerQuestsDictionary();

        public void Unsubscribe() =>
            OnChangeQuestState = null;

        public void ChangeState(QuestId id, QuestState state)
        {
            _questsDictionary.Dictionary[id] = state;
            OnChangeQuestState?.Invoke();
            OnChangeQuestStateSendInfo?.Invoke(id, state);
        }

        public QuestState GetState(QuestId id) =>
            _questsDictionary.Dictionary.TryGetValue(id, out var state) ? state : QuestState.None;
    }
}