using UnityEngine;

namespace Plugins.Creator_Kit___RPG.Scripts.Gameplay
{
    /// <summary>
    /// A choice in a conversation script.
    /// </summary>
    [System.Serializable]
    public struct ConversationOption
    {
        public string text;
        public Sprite image;
        public AudioClip audio;
        public string targetId;
        public bool enabled;
    }
}