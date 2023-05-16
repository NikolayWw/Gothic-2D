namespace CodeBase.UI.Windows.Dialog.Logic
{
    public struct DialogData
    {
        public bool IsGGFocus { get; }  //Player
        public string NpcName { get; }
        public string Dialog { get; }
        public string AudioName { get; }

        /// <summary>
        /// Gg is a player
        /// </summary>
        /// <param name="isGgFocus"></param>
        public DialogData(bool isGgFocus, string npcName, string dialog, string audioName)
        {
            IsGGFocus = isGgFocus;
            NpcName = npcName;
            Dialog = dialog;
            AudioName = audioName;
        }
    }
}