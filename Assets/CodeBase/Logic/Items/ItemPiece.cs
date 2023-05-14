using CodeBase.Data.PlayerProgress.WorldData.ItemPiece;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData.Items;
using System;
using UnityEngine;

namespace CodeBase.Logic.Items
{
    public class ItemPiece : MonoBehaviour
    {
        [field: SerializeField] public ItemId Id { get; private set; }
        [field: SerializeField] public int Amount { get; private set; } = 1;
        public Action Oncollected;
        private ItemPieceDataDictionary _itemsDictionary;
        public bool IsPicked { get; private set; }
        private string _uniqueId;

        public void Construct(IPersistentProgressService persistentProgressService, ItemId id, int amount)
        {
            _itemsDictionary = persistentProgressService.PlayerProgress.WorldData.ItemPieceDataDictionary;
            Id = id;
            Amount = amount;
        }

        public void Initialize(in string uniqueId)
        {
            _uniqueId = uniqueId;
            UpdateProgressData();
        }

        private void OnValidate()
        {
            if (Amount < 1) Amount = 1;
        }

        public void SetAmount(int amount)
        {
            Amount = amount;
            UpdateProgressData();
        }

        public void Picked()
        {
            if (IsPicked == false)
            {
                IsPicked = true;
                Oncollected?.Invoke();
                RemoveFromProgressData();
                Destroy(gameObject);
            }
        }

        private void UpdateProgressData()
        {
            _itemsDictionary.Dictionary[_uniqueId] = new ItemPieceData(_uniqueId, Id, Amount, transform.position);
        }

        private void RemoveFromProgressData()
        {
            if (_itemsDictionary.Dictionary.ContainsKey(_uniqueId))
                _itemsDictionary.Dictionary.Remove(_uniqueId);
        }
    }
}