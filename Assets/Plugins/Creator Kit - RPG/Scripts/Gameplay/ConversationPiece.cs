using System.Collections.Generic;
using UnityEngine;

namespace Plugins.Creator_Kit___RPG.Scripts.Gameplay
{
    /// <summary>
    /// A single dialog of text and  options in a conversation script.
    /// If quest is set, this dialog will start a quest.
    /// If audio is set, the clip will play when this conversation piece is
    /// displayed.
    /// </summary>
    [System.Serializable]
    public struct ConversationPiece
    {
        public string id;

        [Multiline]
        public string text;

        public Sprite image;
        public AudioClip audio;
        public Quest quest;
        public List<ConversationOption> options;
    }
}