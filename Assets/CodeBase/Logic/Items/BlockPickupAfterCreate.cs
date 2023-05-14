#region

using CodeBase.Infrastructure.Logic;

#endregion

namespace CodeBase.Logic.Items
{
    public class BlockPickupAfterCreate
    {
        private readonly Timer _timer;
        public bool IsBlocked => _timer.IsTimerElapsed == false;

        public BlockPickupAfterCreate(ICoroutineRunner coroutineRunner) =>
            _timer = new Timer(coroutineRunner);

        public void StartBlockTimer() =>
            _timer.StartTimer(0.5f); //delay
    }
}