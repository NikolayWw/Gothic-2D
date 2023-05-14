using CodeBase.Data.PlayerProgress;
using CodeBase.Services.GameFactory;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using UnityEngine;

namespace CodeBase.Player.UseItems
{
    public class UseItemsInInventoryContainer : MonoBehaviour, ISaveLoad
    {
        [SerializeField] private Transform _weaponAnchor;
        public EquipItems EquipItems { get; private set; }
        public ConsumeItems ConsumeItems { get; private set; }

        public void Construct(IGameFactory gameFactory, IStaticDataService dataService, IPersistentProgressService persistentProgressService)
        {
            EquipItems = new EquipItems(_weaponAnchor, gameFactory, dataService, persistentProgressService);
            ConsumeItems = new ConsumeItems(dataService, persistentProgressService);
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            EquipItems.UpdateProgress(progress);
        }

        public void LoadProgress(PlayerProgress progress)
        {
            EquipItems.LoadProgress(progress);
        }
    }
}