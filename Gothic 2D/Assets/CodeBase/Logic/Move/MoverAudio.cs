#region

using CodeBase.Services.StaticData;
using CodeBase.StaticData.Audio;
using UnityEngine;

#endregion

namespace CodeBase.Logic.Move
{
    public class MoverAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource _playOneShotAudioSource;
        [SerializeField] private AudioSource _moveAudioSource;

        private ILookDirection _lookDirection;
        private IStaticDataService _staticDataService;

        public void Construct(ILookDirection lookDirection, IStaticDataService staticDataService)
        {
            _lookDirection = lookDirection;
            _staticDataService = staticDataService;

            _moveAudioSource.clip = _staticDataService.ForAudio(AudioId.PlayerMove);
        }

        private void Update()
        {
            UpdateAudio();
        }

        public void PlaySwing() =>
            _playOneShotAudioSource.PlayOneShot(_staticDataService.ForAudio(AudioId.PlayerSwing));

        public void PlayHit() =>
            _playOneShotAudioSource.PlayOneShot(_staticDataService.ForAudio(AudioId.PlayerHit));

        private void UpdateAudio()
        {
            if (_lookDirection.IsMoving())
            {
                if (_moveAudioSource.isPlaying == false)
                    _moveAudioSource.Play();
            }
            else if (_moveAudioSource.isPlaying)
            {
                _moveAudioSource.Stop();
            }
        }
    }
}