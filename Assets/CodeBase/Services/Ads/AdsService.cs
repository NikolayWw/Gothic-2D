using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace CodeBase.Services.Ads
{
    public class AdsService : IAdsService, IUnityAdsListener
    {
        private const string AndroidGameId = "5264490";
        private const string IOSGameId = "5264491";

        private const string RewardedPlacementIdAndroid = "Rewarded_Android";
        private const string RewardedPlacementIdIOS = "Rewarded_iOS";

        public bool IsRewardedVideoReady => Advertisement.IsReady(_placementI);
        public Action RewardedVideoReady { get; set; }

        private Action _onVideoFinished;
        private string _gameId = string.Empty;
        private string _placementI = string.Empty;

        public void Initialize()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    _gameId = IOSGameId;
                    _placementI = RewardedPlacementIdIOS;
                    break;

                case RuntimePlatform.Android:
                    _gameId = AndroidGameId;
                    _placementI = RewardedPlacementIdAndroid;
                    break;

                case RuntimePlatform.WindowsEditor:
                    _gameId = AndroidGameId;
                    _placementI = RewardedPlacementIdAndroid;
                    break;

                default:
                    Debug.Log("Unsupported platform for ads");
                    break;
            }

            Advertisement.AddListener(this);
            Advertisement.Initialize(_gameId);
        }

        public void ShowRewardedVideo(Action onVideoFinished)
        {
            Advertisement.Show(_placementI);
            _onVideoFinished = onVideoFinished;
        }

        public void OnUnityAdsReady(string placementId)
        {
            if (placementId == _placementI)
                RewardedVideoReady?.Invoke();
        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            switch (showResult)
            {
                case ShowResult.Failed:
                    Debug.LogError($"OnUnityAdsDidFinish {showResult}");
                    break;

                case ShowResult.Skipped:
                    Debug.LogError($"OnUnityAdsDidFinish {showResult}");
                    break;

                case ShowResult.Finished:
                    _onVideoFinished?.Invoke();
                    break;

                default:
                    Debug.LogError($"OnUnityAdsDidFinish {showResult}");
                    break;
            }

            _onVideoFinished = null;
        }

        public void OnUnityAdsDidError(string message)
        { }

        public void OnUnityAdsDidStart(string placementId)
        { }
    }
}