using CodeBase.Data.PlayerProgress;

namespace CodeBase.Services.PersistentProgress
{
    public interface ISaveLoad
    {
        void UpdateProgress(PlayerProgress progress);

        void LoadProgress(PlayerProgress progress);
    }
}