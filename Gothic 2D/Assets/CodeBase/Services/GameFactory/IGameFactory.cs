#region

using CodeBase.Data.PlayerProgress.Npc;
using CodeBase.Logic.Items;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData.Items;
using CodeBase.StaticData.Npc;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Window;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

#endregion

namespace CodeBase.Services.GameFactory
{
    public interface IGameFactory : IService
    {
        Task<GameObject> CreatePlayer(Vector2 position, IWindowService windowService,
            PlayerInteractionButton playerInteractionButton);

        Task CreateCMVcam(Transform player, Collider2D confinerCollider);

        Task CreateNpcSpawner(NpcId id, Vector2 at, IUIFactory uiFactory, IWindowService windowService);

        Task<GameObject> CreateNpc(NpcId id, Transform parent, NpcData npcData, IUIFactory uiFactory, IWindowService windowService);

        GameObject Player { get; }
        List<ISaveLoad> SaveProgress { get; }

        Task<ItemPiece> CreateItemPiece(ItemId itemId, int amount, Vector2 at);

        Task CreateMeleeWeaponInHand(AssetReferenceGameObject meleeWeaponPrefab, Transform parent);

        void ClearMeleeWeaponInHand();

        Task CreateItemSpawner(string uniqueId, ItemId id, int amount, Vector2 at);

        void Clean();

        Task Warmup();
    }
}