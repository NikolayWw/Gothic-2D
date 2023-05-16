using System;

namespace CodeBase.Services.Ads
{
    public interface IAdsService : IService
    {
        bool IsRewardedVideoReady { get; }
        Action RewardedVideoReady { get; set; }
        void Initialize();
        void ShowRewardedVideo(Action onVideoFinished);
    }
}