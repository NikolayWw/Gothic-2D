using CodeBase.Data.PlayerProgress;
using System;

namespace CodeBase.Services.SaveLoad
{
    public interface ISaveLoadService : IService
    {
        void SaveProgress(int progressIndex);

        PlayerProgress[] PlayerProgresses { get; }
        Action OnLoadAllProgress { get; set; }

        void LoadAllProgress();

        PlayerProgress NewProgress();

        void LoadProgress(int progressIndex);

        void ClearProgress(int progressIndex);
    }
}