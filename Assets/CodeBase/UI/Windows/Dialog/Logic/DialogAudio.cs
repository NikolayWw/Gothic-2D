using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.UI.Windows.Dialog.Logic
{
    public class DialogAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        public void Play(AudioClip audioClip, Action finishedPlaying)
        {
            _audioSource.PlayOneShot(audioClip);
            StartCoroutine(CheckPlayingAudio(finishedPlaying));
        }

        private IEnumerator CheckPlayingAudio(Action finishedPlaying)
        {
            while (_audioSource.isPlaying)
                yield return null;
            finishedPlaying?.Invoke();
        }
    }
}