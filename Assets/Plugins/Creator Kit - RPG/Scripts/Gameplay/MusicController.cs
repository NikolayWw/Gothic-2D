using UnityEngine;
using UnityEngine.Audio;

namespace Plugins.Creator_Kit___RPG.Scripts.Gameplay
{
    public class MusicController : MonoBehaviour
    {
        public AudioMixerGroup audioMixerGroup;
        public AudioClip audioClip;
        public float crossFadeTime = 3;

        private AudioSource audioSourceA, audioSourceB;
        private float audioSourceAVolumeVelocity, audioSourceBVolumeVelocity;

        public void CrossFade(AudioClip audioClip)
        {
            var t = audioSourceA;
            audioSourceA = audioSourceB;
            audioSourceB = t;
            audioSourceA.clip = audioClip;
            audioSourceA.Play();
        }

        private void Update()
        {
            audioSourceA.volume = Mathf.SmoothDamp(audioSourceA.volume, 1f, ref audioSourceAVolumeVelocity, crossFadeTime, 1);
            audioSourceB.volume = Mathf.SmoothDamp(audioSourceB.volume, 0f, ref audioSourceBVolumeVelocity, crossFadeTime, 1);
        }

        private void OnEnable()
        {
            audioSourceA = gameObject.AddComponent<AudioSource>();
            audioSourceA.spatialBlend = 0;
            audioSourceA.clip = audioClip;
            audioSourceA.loop = true;
            audioSourceA.outputAudioMixerGroup = audioMixerGroup;
            audioSourceA.Play();

            audioSourceB = gameObject.AddComponent<AudioSource>();
            audioSourceB.spatialBlend = 0;
            audioSourceB.loop = true;
            audioSourceB.outputAudioMixerGroup = audioMixerGroup;
        }
    }
}