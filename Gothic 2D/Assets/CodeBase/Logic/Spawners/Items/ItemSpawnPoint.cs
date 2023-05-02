using CodeBase.Data.PlayerProgress;
using CodeBase.Logic.Items;
using CodeBase.Services.GameFactory;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData.Items;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Logic.Spawners.Items
{
    public class ItemSpawnPoint : MonoBehaviour
    {
        private PlayerProgress _playerProgress;
        private string _uniqueId;
        private IGameFactory _factory;
        private ItemId _id;
        private int _amount;

        public void Construct(in string uniqueId, ItemId id, int amount, IPersistentProgressService persistentProgressService, IGameFactory factory)
        {
            _uniqueId = uniqueId;
            _id = id;
            _amount = amount;
            _playerProgress = persistentProgressService.PlayerProgress;
            _factory = factory;
        }

        private async void Start()
        {
            if (_playerProgress.KillData.ItemSpawner.Contains(_uniqueId) == false)
                await Create();
        }

        private async Task Create()
        {
            ItemPiece itemPiece = await _factory.CreateItemPiece(_id, _amount, transform.position);
            itemPiece.Initialize(_uniqueId);
            _playerProgress.KillData.ItemSpawner.Add(_uniqueId);
        }
    }
}