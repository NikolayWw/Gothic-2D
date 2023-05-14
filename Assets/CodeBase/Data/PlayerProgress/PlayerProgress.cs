using CodeBase.Data.PlayerProgress.Chest;
using CodeBase.Data.PlayerProgress.Npc;
using CodeBase.Data.PlayerProgress.Player;
using CodeBase.Data.PlayerProgress.Player.Quests;
using System;
using UnityEngine;

namespace CodeBase.Data.PlayerProgress
{
    [Serializable]
    public class PlayerProgress
    {
        [field: SerializeField] public string ProgressName { get; private set; }
        public PlayerData PlayerData;

        public PlayerQuestContainer QuestContainer;

        public WorldData.WorldData WorldData;
        public NpcDataDictionary NpcDataDictionary;
        public ChestData ChestData;
        public KillData KillData;

        public PlayerProgress(PlayerData playerData, in string sceneKey, in string progressName, Vector2 playerInitialPosition)
        {
            ProgressName = progressName;
            PlayerData = playerData;
            WorldData = new WorldData.WorldData(sceneKey, playerInitialPosition);
            NpcDataDictionary = new NpcDataDictionary();
            ChestData = new ChestData();
            KillData = new KillData();
            QuestContainer = new PlayerQuestContainer();
        }

        public void SetProgressName(in string name) =>
            ProgressName = name;
    }
}