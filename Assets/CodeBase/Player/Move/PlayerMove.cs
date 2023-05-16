#region

using CodeBase.Data.PlayerProgress;
using CodeBase.Data.PlayerProgress.WorldData;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

namespace CodeBase.Player.Move
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMove : MonoBehaviour, ISaveLoad
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private PlayerCheckDead _playerCheckDead;
        [SerializeField] private float _speed;
        private IInputService _inputService;

        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
            _playerCheckDead.Happened += Pause;

            Debug.Log("Construct Test");
        }

        #region Test 1/2
        private bool _toggleTest;
        #endregion

        private void FixedUpdate()
        {
            #region Test 2/2
            if (_toggleTest == false)
            {
                _toggleTest = true;
                Debug.Log("Fix Test");
            }
            #endregion
            Move(_inputService.MoveAxis);
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.WorldData.PositionOnLevel = new PositionOnLevel(GetSceneKey(), transform.position);
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (GetSceneKey() == progress.WorldData.PositionOnLevel.Level)
            {
                transform.position = progress.WorldData.PositionOnLevel.Position;
            }
        }

        private static string GetSceneKey()
        {
            return SceneManager.GetActiveScene().name;
        }

        private void Pause()
        {
            enabled = false;
            _rigidbody.velocity *= 0;
        }

        private void Move(Vector2 axis)
        {
            _rigidbody.velocity = axis * _speed;
        }
    }
}