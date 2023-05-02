using System;
using UnityEngine;

namespace CodeBase.StaticData.Dialog
{
    [CreateAssetMenu(fileName = "New Speeches", menuName = "Static Data/Dialog/Speech", order = 0)]
    public class DialogAudioStaticData : ScriptableObject
    {
    }

    [Serializable]
    public struct DialogAudioConfig
    {
        [field: SerializeField] public AudioClip AudioClip { get; private set; }
    }
}