using CodeBase.StaticData.Dialog;
using System;
using System.Collections.Generic;

namespace CodeBase.UI.Windows.Dialog.Logic
{
    public struct DialogData
    {
        public List<DialogId> SpeechIds { get; }
        public Action OnEndPlaySpeech { get; }
        public string NpcName { get; }

        public DialogData(string npcName, List<DialogId> speechIds, Action onEndPlaySpeech)
        {
            SpeechIds = speechIds;
            NpcName = npcName;
            OnEndPlaySpeech = onEndPlaySpeech;
        }
    }
}