#region

using UnityEngine;

#endregion

namespace CodeBase.Logic.Move
{
    public class MoverAnimation : MonoBehaviour
    {
        private readonly int AttackAnimationHash = Animator.StringToHash("Attack");
        private readonly int DeadAnimationHash = Animator.StringToHash("Dead");

        private readonly int XLookHash = Animator.StringToHash("XLook");
        private readonly int YLookHash = Animator.StringToHash("YLook");
        private readonly int MoveHash = Animator.StringToHash("Move");
        private readonly int HurtHash = Animator.StringToHash("Hurt");

        [SerializeField] private Animator _animator;
        private ILookDirection _lookDirection;

        public void Construct(ILookDirection lookDirection)
        {
            _lookDirection = lookDirection;
        }

        private void Update()
        {
            UpdateMoving();
            UpdateLookDirection();
        }

        private void UpdateMoving()
        {
            _animator.SetBool(MoveHash, _lookDirection.IsMoving());
        }

        private void UpdateLookDirection()
        {
            _animator.SetFloat(XLookHash, _lookDirection.LookDirection.x);
            _animator.SetFloat(YLookHash, _lookDirection.LookDirection.y);
        }

        public void PlayAttack()
        {
            _animator.Play(AttackAnimationHash, 0, 0);
        }

        public void PlayDead()
        {
            _animator.Play(DeadAnimationHash, 0, 0);
        }

        public void PlayHurt()
        {
            //_animator.Play(HurtHash);
        }
    }
}