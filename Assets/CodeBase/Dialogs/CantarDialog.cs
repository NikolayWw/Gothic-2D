using CodeBase.StaticData.Dialog;

namespace CodeBase.Dialogs
{
    public class CantarDialog : BaseDialog
    {
        protected override string NpcName { get; set; } = "Кантар";

        protected override void AddButtons()
        {
            SpeechInfoButtons.Add(TRADEInfo);
        }

        private void TRADEInfo()
        {
            if (TRADEInfoCondition() == false)
                return;

            CreateStartSpeechButton(TRADE, "Покажи мне свои товары!", "TRADEInfo");
        }

        private bool TRADEInfoCondition()
        {
            return true;
        }

        private void TRADE()
        {
            AddContext(DialogId.LogartPro, DialogId.LogartPro);

            void Action()
            {
                Shop.Open();
                EndDialog();
            }
            Play(Action);
        }
    }
}