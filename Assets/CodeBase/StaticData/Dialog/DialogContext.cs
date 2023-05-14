using System;
using UnityEngine;

namespace CodeBase.StaticData.Dialog
{
    [Serializable]
    public class DialogContext
    {
        [field: SerializeField] public string Context { get; private set; }
        [field: SerializeField] public bool IsGGTalking { get; private set; }
        [field: SerializeField] public AudioClip Audio { get; private set; }
    }
}