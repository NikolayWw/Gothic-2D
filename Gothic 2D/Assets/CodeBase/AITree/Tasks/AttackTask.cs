using CodeBase.AITree.Tree;
using CodeBase.Data.PlayerProgress.Player;
using CodeBase.Infrastructure.Logic;
using CodeBase.Logic;
using CodeBase.Logic.Move;
using CodeBase.Npc;
using CodeBase.Services.PersistentProgress;

namespace CodeBase.AITree.Tasks
{
    public class AttackTask : Node
    {
        private readonly NpcBehaviour _npcBehaviour;
        private readonly Timer _timer;
        private readonly MoverAnimation _animation;
        private readonly PlayerCharacteristics _playerCharacteristics;

        public AttackTask(NpcBehaviour npcBehaviour, ICoroutineRunner coroutineRunner, MoverAnimation animation, IPersistentProgressService persistentProgressService)
        {
            _npcBehaviour = npcBehaviour;
            _timer = new Timer(coroutineRunner);
            _animation = animation;
            _playerCharacteristics = persistentProgressService.PlayerProgress.PlayerData.PlayerCharacteristics;
        }

        public override bool Evaluate()
        {
            var target = GetTarget();
            if (target == null)
                return false;

            if (_timer.IsTimerElapsed)
            {
                _timer.StartTimer(0.4f);
                _animation.PlayAttack();
                _playerCharacteristics.DecrementHealth(5);
                if (_playerCharacteristics.CurrentHealth == 0)
                {
                    _npcBehaviour.SetTarget(null);
                    ClearData();
                    return false;
                }
            }

            return true;
        }
    }
}