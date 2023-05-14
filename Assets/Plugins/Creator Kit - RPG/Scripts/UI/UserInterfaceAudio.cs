using System.Collections.Generic;
using UnityEngine;

namespace Plugins.Creator_Kit___RPG.Scripts.UI
{
    [RequireComponent(typeof(AudioSource))]
    public class UserInterfaceAudio : MonoBehaviour
    {
        private static UserInterfaceAudio instance;

        public AudioClip onButtonClick, onButtonEnter, onButtonExit;
        public AudioClip onShowDialog, onHideDialog;
        public AudioClip onCollect;
        public AudioClip onStoryItem;

        public AudioClip[] vocals;

        private AudioSource audioSource;
        private AudioClip speech;

        private struct SpeechSyllable
        {
            public int index;
            public float time;
        }

        private Queue<SpeechSyllable> syllables = new Queue<SpeechSyllable>();

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            if (instance != null)
                Destroy(instance);
            else
                instance = this;
        }

        private void Update()
        {
            if (syllables.Count > 0 && syllables.Peek().time < Time.time)
            {
                var s = syllables.Dequeue();
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                Play(vocals[s.index]);
            }
        }

        private void PlaySpeech(int seed, int syllableCount, float pitch)
        {
            Random.InitState(seed);
            var now = Time.time;
            for (var i = 0; i < syllableCount; i++)
            {
                now += Random.Range(0.1f, 0.3f);
                syllables.Enqueue(new SpeechSyllable() { index = Random.Range(0, vocals.Length), time = now });
            }
        }

        public static void Speak(int seed, int syllables, float pitch)
        {
            if (instance != null)
                instance.PlaySpeech(seed, syllables, pitch);
        }

        public static void OnCollect()
        {
            if (instance != null) instance.Play(instance.onCollect);
        }

        public static void OnButtonEnter()
        {
            if (instance != null) instance.Play(instance.onButtonEnter);
        }

        public static void OnButtonExit()
        {
            if (instance != null) instance.Play(instance.onButtonExit);
        }

        internal static void OnStoryItem()
        {
            if (instance != null) instance.Play(instance.onStoryItem);
        }

        public static void OnShowDialog()
        {
            if (instance != null) instance.Play(instance.onShowDialog);
        }

        public static void OnButtonClick()
        {
            if (instance != null) instance.Play(instance.onButtonClick);
        }

        private void Play(AudioClip clip)
        {
            if (clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
        }

        public static void OnHideDialog()
        {
            if (instance != null)
                instance.Play(instance.onHideDialog);
        }

        public static void PlayClip(AudioClip audioClip)
        {
            if (instance != null) instance.Play(audioClip);
        }
    }
}