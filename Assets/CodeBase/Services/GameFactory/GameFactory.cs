using Cinemachine;
using CodeBase.Data.PlayerProgress;
using CodeBase.Data.PlayerProgress.Npc;
using CodeBase.Dialogs;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Inventory;
using CodeBase.Logic;
using CodeBase.Logic.Items;
using CodeBase.Logic.Move;
using CodeBase.Logic.Spawners.Items;
using CodeBase.Logic.Spawners.Npc;
using CodeBase.Npc;
using CodeBase.Player;
using CodeBase.Player.Move;
using CodeBase.Player.UseItems;
using CodeBase.Services.Input;
using CodeBase.Services.LogicFactory;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Items;
using CodeBase.StaticData.Npc;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Window;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Services.GameFactory
{
    public class GameFactory : IGameFactory
    {
        private readonly NpcId[] WhiteBanditsList = { NpcId.Bandit_Cavalorn1, NpcId.Bandit_Cavalorn2 };

        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly IInputService _inputService;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly ILogicFactoryService _logicFactory;
        public List<ISaveLoad> SaveProgress { get; } = new List<ISaveLoad>();
        public GameObject Player { get; private set; }
        private GameObject _currentMeleeWeapon;

        public GameFactory(IAssetProvider assetProvider, IStaticDataService staticDataService, IInputService inputService, IPersistentProgressService persistentProgressService, ILogicFactoryService logicFactory)
        {
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
            _inputService = inputService;
            _persistentProgressService = persistentProgressService;
            _logicFactory = logicFactory;
        }

        public void Clean()
        {
            SaveProgress.Clear();
        }

        public async Task Warmup()
        {
            await _assetProvider.Load<GameObject>(AssetsAddress.NpcSpawnPoint);
            await _assetProvider.Load<GameObject>(AssetsAddress.ItemSpawnPoint);
        }

        public async Task CreateMeleeWeaponInHand(AssetReferenceGameObject meleeWeaponReference, Transform parent)
        {
            var prefab = await _assetProvider.Load<GameObject>(meleeWeaponReference);
            _currentMeleeWeapon = Object.Instantiate(prefab, parent);
        }

        public void ClearMeleeWeaponInHand()
        {
            if (_currentMeleeWeapon)
            {
                Object.Destroy(_currentMeleeWeapon);
                _currentMeleeWeapon = null;
            }
        }

        public async Task<GameObject> CreatePlayer(Vector2 position, IWindowService windowService, PlayerInteractionButton playerInteractionButton)
        {
            GameObject player = await _assetProvider.Instantiate(AssetsAddress.Player, position);

            player.GetComponent<PlayerMove>()?.Construct(_inputService);
            PlayerLookDirection lookDirection = player.GetComponent<PlayerLookDirection>();
            lookDirection.Construct(_inputService);
            player.GetComponent<MoverAudio>()?.Construct(lookDirection, _staticDataService);
            player.GetComponent<MoverAnimation>()?.Construct(lookDirection);
            player.GetComponent<UseItemsInInventoryContainer>()?.Construct(this, _staticDataService, _persistentProgressService);
            player.GetComponent<PlayerKiller>()?.Construct(_persistentProgressService, _staticDataService);
            player.GetComponent<PlayerInteractionInWorld>()?.Construct(playerInteractionButton);
            player.GetComponent<PlayerCheckDead>()?.Construct(_persistentProgressService);
            await CreateUpdateHealth(_persistentProgressService.PlayerProgress.PlayerData.PlayerCharacteristics, player.transform);

            Register(player);

            Player = player;
            return player;
        }

        public async Task CreateCMVcam(Transform player, Collider2D confinerCollider)
        {
            var prefab = await _assetProvider.Load<GameObject>(AssetsAddress.CVcam);
            GameObject instance = Object.Instantiate(prefab);

            if (instance.TryGetComponent(out CinemachineVirtualCamera cinemachine))
                cinemachine.Follow = player;
            if (instance.TryGetComponent(out CinemachineConfiner2D confiner))
                confiner.m_BoundingShape2D = confinerCollider;
        }

        public async Task<ItemPiece> CreateItemPiece(ItemId itemId, int amount, Vector2 at)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(GetItem(itemId).PrefabPieceReference);
            ItemPiece itemPiece = Object.Instantiate(prefab, at, quaternion.identity).GetComponent<ItemPiece>();

            itemPiece.transform.position = at;
            itemPiece.Construct(_persistentProgressService, itemId, amount);

            itemPiece.gameObject.GetComponent<ItemPickup>()?.Construct(_persistentProgressService, _logicFactory);
            return itemPiece;
        }

        public async Task CreateItemSpawner(string uniqueId, ItemId id, int amount, Vector2 at)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(AssetsAddress.ItemSpawnPoint);
            ItemSpawnPoint spawnPoint = Object.Instantiate(prefab, at, Quaternion.identity).GetComponent<ItemSpawnPoint>();
            spawnPoint.Construct(uniqueId, id, amount, _persistentProgressService, this);
        }

        public async Task CreateNpcSpawner(NpcId id, Vector2 at, IUIFactory uiFactory, IWindowService windowService)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(AssetsAddress.NpcSpawnPoint);

            NpcSpawnPoint spawnPoint = Object.Instantiate(prefab, at, Quaternion.identity).GetComponent<NpcSpawnPoint>();
            Register(spawnPoint.gameObject);
            spawnPoint.Construct(id, _persistentProgressService, this, _staticDataService, uiFactory, windowService);
        }

        public async Task<GameObject> CreateNpc(NpcId id, Transform parent, NpcData npcData, IUIFactory uiFactory, IWindowService windowService)
        {
            NpcConfig config = _staticDataService.ForNpc(id);
            GameObject prefab = await _assetProvider.Load<GameObject>(config.TemplateReference);

            GameObject npc = Object.Instantiate(prefab, parent);
            NpcMover lookDirection = npc.GetComponent<NpcMover>();
            npc.GetComponent<MoverAnimation>()?.Construct(lookDirection);
            npc.GetComponent<NpcBehaviour>()?.Construct(_persistentProgressService, _logicFactory, npcData?.NpcCharacteristics);
            npc.GetComponentInChildren<NpcHealth>()?.Construct(npcData?.NpcCharacteristics);
            npc.GetComponent<BaseDialog>()?.Construct(uiFactory, npcData, _persistentProgressService, this, _staticDataService, windowService, _logicFactory);
            npc.GetComponent<OpenBox>()?.Construct(id.ToString(), _logicFactory, _persistentProgressService, _staticDataService);
            if (WhiteBanditsList.Contains(id))
            {
                await CreateUpdateHealth(npcData?.NpcCharacteristics, npc.transform);
                Register(npc);
            }
            return npc;
        }

        private async Task CreateUpdateHealth(IDataHealth dataHealth, Transform followTarget)
        {
            var prefab = await _assetProvider.Load<GameObject>(AssetsAddress.UpdateUIHealth);
            UpdateUIHealth health = Object.Instantiate(prefab).GetComponent<UpdateUIHealth>();
            health.Construct(dataHealth, followTarget);
        }

        private void Register(GameObject obj)
        {
            foreach (ISaveLoad component in obj.GetComponents<ISaveLoad>())
                SaveProgress.Add(component);
        }

        private BaseItem GetItem(ItemId id) =>
            _staticDataService.ForItem(id);
    }
}