using System;
using UnityEngine;

namespace CodeBase.StaticData.Audio
{
    [Serializable]
    public class AudioConfig
    {
        [SerializeField] private string _inspectorName = string.Empty;
        [field: SerializeField] public AudioId Id { get; private set; }
        [field: SerializeField] public AudioClip AudioClip { get; private set; }

        public void OnValidate() =>
            _inspectorName = Id.ToString();
    }
}