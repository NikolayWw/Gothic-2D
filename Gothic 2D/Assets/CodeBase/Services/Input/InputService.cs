#region

using UnityEngine;

#endregion

namespace CodeBase.Services.Input
{
    public class InputService : IInputService
    {
        public Vector2 MoveAxis => _mainControls.Player.Move.ReadValue<Vector2>();
        private readonly MainControls _mainControls;

        public InputService()
        {
            _mainControls = new MainControls();
            EnableActions();
        }

        private void EnableActions()
        {
            _mainControls.Player.Move.Enable();
        }
    }
}