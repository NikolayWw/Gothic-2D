using CodeBase.Logic;
using CodeBase.Services.LogicFactory;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using UnityEngine;

namespace CodeBase.Inventory
{
    public class InitInventoryBox : MonoBehaviour
    {
        [SerializeField] private OpenBox _box;
        [SerializeField] private UniqueId _uniqueId;

        public void Initialize(ILogicFactoryService factoryService, IPersistentProgressService persistentProgressService, IStaticDataService dataService)
        {
            _box.Construct(_uniqueId.Id, factoryService, persistentProgressService, dataService);
        }
    }
}