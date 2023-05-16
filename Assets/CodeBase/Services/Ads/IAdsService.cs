using System;

namespace CodeBase.Services.Ads
{
    public interface IAdsService : IService
    {
        Action RewardedVideoReady { get; set; }
        void Initialize();
        void ShowRewardedVideo(Action onVideoFinished);
    }
}