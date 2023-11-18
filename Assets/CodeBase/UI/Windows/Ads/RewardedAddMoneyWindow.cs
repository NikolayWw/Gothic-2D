using CodeBase.Data.PlayerProgress.InventoryData;
using CodeBase.Inventory;
using CodeBase.Services.LogicFactory;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Ads;
using CodeBase.StaticData.Items;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Ads
{
    public class RewardedAddMoneyWindow : BaseWindow
    {
        [SerializeField] private Image _adsSliderImage;

        private AdsConfig _config;
        private InventorySlotsHandler _inventoryHandler;
        private InventorySlotsContainer _playerInventory;

        private bool _adsReady;

        public void Construct(IStaticDataService dataService, ILogicFactoryService logicFactory, IPersistentProgressService persistentProgressService)
        {
            _config = dataService.AdsStaticData;
            _inventoryHandler = logicFactory.InventorySlotsHandler;
            _playerInventory = persistentProgressService.PlayerProgress.PlayerData.SlotsContainer;
        }

        private void Start()
        {
            StartTimer();
        }

        private void StartTimer()
        {
            _adsReady = false;
            StartCoroutine(AdsTimer());
        }

        private async void OnVideoFinished()
        {
            await _inventoryHandler.AddOrDropItem(ItemId.Gold, _config.RewardedGoldAmount, _playerInventory);
            _adsReady = false;
            StartTimer();
        }

        private IEnumerator AdsTimer()
        {
            float time = 0f;
            while (time < _config.AdsDelay)
            {
                time += Time.deltaTime;
                _adsSliderImage.fillAmount = time / _config.AdsDelay;
                yield return null;
            }

            _adsReady = true;
        }
    }
}