using CodeBase.StaticData.Ads;
using CodeBase.StaticData.Audio;
using CodeBase.StaticData.Dialog;
using CodeBase.StaticData.GamePlayMessage;
using CodeBase.StaticData.Inventory;
using CodeBase.StaticData.Items;
using CodeBase.StaticData.Level;
using CodeBase.StaticData.Npc;
using CodeBase.StaticData.Player;
using CodeBase.StaticData.SaveLoad;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Window;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeBase.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        #region Address

        private const string WindowStaticDataAddress = "WindowStaticData";
        private const string NpcStaticDataAddress = "NpcContainerStaticData";
        private const string ItemContainerAddress = "ItemContainer";
        private const string LevelDataAddress = "LevelDataContainer";
        private const string InventoryConfigAddress = "InventoryConfig";
        private const string PlayerStaticDataAddress = "PlayerData";
        private const string AudioDataAddress = "AudioStaticData";
        private const string DialogStaticDataAddress = "DialogStaticData";
        private const string LoadSaveDataAddress = "UISaveLoadStaticData";
        private const string GameplayMessageDataAddress = "GameplayMessageStaticData";
        private const string AdsStaticDataAddress = "AdsConfig";

        #endregion Address

        public GameplayMessageStaticData GameplayMessageData { get; private set; }
        public DialogStaticData DialogStaticData { get; private set; }
        public InventoryConfig InventoryConfig { get; private set; }
        public PlayerStaticData PlayerStaticData { get; private set; }
        public UILoadSaveStaticData UILoadSaveStaticData { get; private set; }
        public AdsConfig AdsStaticData { get; private set; }
        private Dictionary<AudioId, AudioConfig> _audioConfigs;
        private Dictionary<WindowId, WindowConfig> _windowConfigs;
        private Dictionary<string, LevelStaticData> _levelsData;
        private Dictionary<ItemId, BaseItem> _items;
        private Dictionary<NpcId, NpcConfig> _npcConfigs;
        private Dictionary<DialogId, List<DialogContext>> _dialogConfigs;
        private List<AsyncOperationHandle> _handlesAddress = new List<AsyncOperationHandle>();

        public async Task Load()
        {
            var windowConfigs = await LoadAsync<WindowStaticData>(WindowStaticDataAddress);
            _windowConfigs = windowConfigs.Configs.ToDictionary(x => x.WindowId, x => x);

            var npcConfigs = await LoadAsync<NpcContainer>(NpcStaticDataAddress);
            _npcConfigs = npcConfigs.Configs.ToDictionary(x => x.Id, x => x);

            var itemContainer = await LoadAsync<ItemContainer>(ItemContainerAddress);
            LoadItems(itemContainer);

            var levelsData = await LoadAsync<LevelStaticDataContainer>(LevelDataAddress);
            _levelsData = levelsData.LevelStaticDatas.ToDictionary(x => x.SceneKey, x => x);

            var audioConfigs = await LoadAsync<AudioStaticData>(AudioDataAddress);
            _audioConfigs = audioConfigs.Configs.ToDictionary(x => x.Id, x => x);

            InventoryConfig = await LoadAsync<InventoryConfig>(InventoryConfigAddress);
            PlayerStaticData = await LoadAsync<PlayerStaticData>(PlayerStaticDataAddress);
            await LoadDialogData();
            GameplayMessageData = await LoadAsync<GameplayMessageStaticData>(GameplayMessageDataAddress);
            UILoadSaveStaticData = await LoadAsync<UILoadSaveStaticData>(LoadSaveDataAddress);
            AdsStaticData = await LoadAsync<AdsConfig>(AdsStaticDataAddress);

            CleanHandlesAddress();
        }

        public LevelStaticData ForLevel(string sceneKey) =>
            _levelsData.TryGetValue(sceneKey, out var data) ? data : null;

        public NpcConfig ForNpc(NpcId id) =>
            _npcConfigs.TryGetValue(id, out var data) ? data : null;

        public AudioClip ForAudio(AudioId id) =>
            _audioConfigs.TryGetValue(id, out var clip) ? clip.AudioClip : null;

        public WindowConfig ForWindow(WindowId id) =>
            _windowConfigs.TryGetValue(id, out var data) ? data : null;

        public BaseItem ForItem(ItemId id) =>
            _items.TryGetValue(id, out var item) ? item : null;

        public List<DialogContext> ForDialogContext(DialogId id) =>
            _dialogConfigs.TryGetValue(id, out var contexts) ? contexts : null;

        private async Task LoadDialogData()
        {
            DialogStaticData = await LoadAsync<DialogStaticData>(DialogStaticDataAddress);
            _dialogConfigs = DialogStaticData.DialogConfigs.ToDictionary(x => x.DialogId, x => x.Contexts);
        }

        private void LoadItems(ItemContainer container)
        {
            _items = new Dictionary<ItemId, BaseItem>
            {
                { container.None.Id,container.None },
                { container.Gold.Id, container.Gold },
            };
            container.OtherItemConfigs.ForEach(x => _items.Add(x.Id, x));
            container.Foods.ForEach(x => _items.Add(x.Id, x));
            container.MeleeWeapons.ForEach(x => _items.Add(x.Id, x));
        }

        private async Task<T> LoadAsync<T>(string address)
        {
            var async = Addressables.LoadAssetAsync<T>(address);
            _handlesAddress.Add(async);
            await async.Task;
            return async.Result;
        }

        private void CleanHandlesAddress()
        {
            _handlesAddress.ForEach(Addressables.Release);
            _handlesAddress = null;
        }
    }
}