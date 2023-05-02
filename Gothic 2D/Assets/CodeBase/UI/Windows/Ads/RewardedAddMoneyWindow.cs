using CodeBase.Data.PlayerProgress.InventoryData;
using CodeBase.Inventory;
using CodeBase.Services.Ads;
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
        [SerializeField] private Button _showAdsButton;

        private AdsConfig _config;
        private IAdsService _adsService;
        private InventorySlotsHandler _inventoryHandler;
        private InventorySlotsContainer _playerInventory;

        private bool _adsReady;

        public void Construct(IStaticDataService dataService, IAdsService adsService, ILogicFactoryService logicFactory, IPersistentProgressService persistentProgressService)
        {
            _config = dataService.AdsStaticData;
            _adsService = adsService;
            _inventoryHandler = logicFactory.InventorySlotsHandler;
            _playerInventory = persistentProgressService.PlayerProgress.PlayerData.SlotsContainer;

            _showAdsButton.onClick.AddListener(OnShowAdClicked);
            _adsService.RewardedVideoReady += RefreshAvailableAd;

            RefreshAvailableAd();
        }

        private void Start()
        {
            StartTimer();
        }

        private void OnDestroy()
        {
            _adsService.RewardedVideoReady -= RefreshAvailableAd;
        }

        private void StartTimer()
        {
            _adsReady = false;
            StartCoroutine(AdsTimer());
        }

        private void RefreshAvailableAd()
        {
            bool ready = _adsReady && _adsService.IsRewardedVideoReady;
            _showAdsButton.interactable = ready;
        }

        private void OnShowAdClicked()
        {
            _adsService.ShowRewardedVideo(OnVideoFinished);
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
            RefreshAvailableAd();
        }
    }
}