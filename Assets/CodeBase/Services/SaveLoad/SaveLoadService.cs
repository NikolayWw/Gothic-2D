using CodeBase.Data;
using CodeBase.Data.PlayerProgress;
using CodeBase.Data.PlayerProgress.InventoryData;
using CodeBase.Data.PlayerProgress.Player;
using CodeBase.Services.GameFactory;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using System;
using UnityEngine;

namespace CodeBase.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string PrefsKey = " " + "GothicSave";

        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly IStaticDataService _dataService;
        public Action OnLoadAllProgress { get; set; }
        public PlayerProgress[] PlayerProgresses { get; private set; } = Array.Empty<PlayerProgress>();

        public SaveLoadService(IGameFactory gameFactory, IPersistentProgressService persistentProgressService, IStaticDataService dataService)
        {
            _gameFactory = gameFactory;
            _persistentProgressService = persistentProgressService;
            _dataService = dataService;
        }

        public void SaveProgress(int progressIndex)
        {
            _gameFactory.SaveProgress.ForEach(x => x.UpdateProgress(_persistentProgressService.PlayerProgress));

            PlayerProgresses[progressIndex] = _persistentProgressService.PlayerProgress;
            _persistentProgressService.PlayerProgress.SetProgressName((progressIndex + 1).ToString());
            SetPrefs(progressIndex);
        }

        public void LoadProgress(int progressIndex)
        {
            PlayerProgresses[progressIndex] = LoadPlayerProgress(progressIndex);
            if (PlayerProgresses[progressIndex] != null)
            {
                InitProgress(PlayerProgresses[progressIndex]);
                _persistentProgressService.PlayerProgress = PlayerProgresses[progressIndex];
            }
        }

        public void LoadAllProgress()
        {
            PlayerProgresses = new PlayerProgress[_dataService.UILoadSaveStaticData.SavesCount];
            for (var i = 0; i < PlayerProgresses.Length; i++)
            {
                PlayerProgresses[i] = LoadPlayerProgress(i);
                if (PlayerProgresses[i] != null)
                    InitProgress(PlayerProgresses[i]);
            }
            OnLoadAllProgress?.Invoke();
        }

        public PlayerProgress NewProgress()
        {
            var playerCharacteristics = new PlayerCharacteristics(_dataService.PlayerStaticData);
            var playerData = new PlayerData(_dataService.InventoryConfig.SlotCount, InventoryType.Player, playerCharacteristics);
            var newPlayerProgress = new PlayerProgress(playerData, GameConstants.MainScene, string.Empty, _dataService.ForLevel(GameConstants.MainScene).PlayerInitialPoint);
            InitProgress(newPlayerProgress);
            return newPlayerProgress;
        }

        public void ClearProgress(int progressIndex)
        {
            PlayerPrefs.DeleteKey(GetProgressKey(progressIndex));

            LoadAllProgress();//refresh progress
        }

        private void InitProgress(PlayerProgress progress)
        {
            progress.PlayerData.SlotsContainer.InitSlots();
            progress.PlayerData.PlayerCharacteristics.SetPlayerStaticData(_dataService.PlayerStaticData);
            foreach (var chestInventory in progress.ChestData.ChestDataDictionary.Dictionary.Values)
                chestInventory.InitSlots();
        }

        private void SetPrefs(int progressIndex)
        {
            string json = JsonUtility.ToJson(PlayerProgresses[progressIndex]);
            PlayerPrefs.SetString(GetProgressKey(progressIndex), json);
        }

        private static PlayerProgress LoadPlayerProgress(int progressIndex)
        {
            string loadedProgress = string.Empty;
            try
            {
                loadedProgress = PlayerPrefs.GetString(GetProgressKey(progressIndex));
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return null;
            }

            try
            {
                return JsonUtility.FromJson<PlayerProgress>(loadedProgress);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return null;
            }
        }

        private static string GetProgressKey(int progressIndex) =>
            progressIndex + PrefsKey;
    }
}