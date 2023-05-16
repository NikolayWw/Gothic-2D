namespace CodeBase.Dialogs
{
    public class CantarDialog : BaseDialog
    {
        protected override string NpcName { get; set; } = "Кантар";

        protected override void AddButtons()
        {
            InfoButtons.Add(TRADEInfo);
        }

        private void TRADEInfo()
        {
            if (TRADEInfoCondition() == false)
                return;

            CreateInput(TRADE, "Покажи мне свои товары!", "TRADEInfo");
        }

        private bool TRADEInfoCondition()
        {
            return true;
        }

        private void TRADE()
        {
            AddContext(true, "Покажи мне свои товары",
                "DIA_Canthar_TRADE_15_00");
            AddContext(false, "Выбирай.",
                "DIA_Canthar_TRADE_09_01");

            void Action()
            {
                Shop.Open();
                EndDialog();
            }
            Play(Action);
        }
    }
}