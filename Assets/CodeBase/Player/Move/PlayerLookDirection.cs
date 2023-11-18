#region

using CodeBase.Logic.Move;
using CodeBase.Services.Input;
using UnityEngine;

#endregion

namespace CodeBase.Player.Move
{
    public class PlayerLookDirection : MonoBehaviour, ILookDirection
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private PlayerCheckDead _playerCheckDead;
        private IInputService _inputService;
        public Vector2 LookDirection { get; private set; } = Vector2.down;
        private float _xScale;

        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
            _playerCheckDead.Happened += Pause;
        }

        private void Start()
        {
            _xScale = transform.localScale.x;
        }

        private void Update()
        {
            UpdateFlip();
            UpdateLookDirection();
        }

        private void Pause()
        {
            enabled = false;
        }

        public bool IsMoving()
        {
            return _rigidbody.velocity.magnitude > 0.2f; //min speed;
        }

        private void UpdateLookDirection()
        {
            if (_inputService == null)
                return;

            LookDirection =
                _inputService.MoveAxis.y > 0 ? Vector2.up :
                _inputService.MoveAxis.y < 0 ? Vector2.down :
                _inputService.MoveAxis.x > 0 ? Vector2.right :
                _inputService.MoveAxis.x < 0 ? Vector2.left :
                LookDirection;
        }

        private void UpdateFlip()
        {
            if(_inputService==null)
                return;

            var scale = transform.localScale;
            scale.x =
                _inputService.MoveAxis.x > 0 ? -_xScale :
                _inputService.MoveAxis.x < 0 ? _xScale :
                scale.x;
            transform.localScale = scale;
        }
    }
}