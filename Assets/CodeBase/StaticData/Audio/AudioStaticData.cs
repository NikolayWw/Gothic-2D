#region

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace CodeBase.StaticData.Audio
{
    [CreateAssetMenu(fileName = "New Audio Data", menuName = "Static Data/AudioData")]
    public class AudioStaticData : ScriptableObject
    {
        [field: SerializeField] public List<AudioConfig> Configs { get; private set; }

        private void OnValidate()
        {
            Configs.ForEach(x => x.OnValidate());
        }
    }
}