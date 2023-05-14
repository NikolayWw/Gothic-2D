using CodeBase.Data.PlayerProgress.Player.Quests;
using CodeBase.StaticData.Dialog;
using CodeBase.StaticData.Items;

namespace CodeBase.Dialogs
{
    public class HildiaDialog : BaseDialog
    {
        #region ExperianceConstants

        protected const int XP_Ambient = 5;
        protected const int XP_LobartHolRueben = 5;
        protected const int XP_Addon_Cavalorn_KillBrago = 5;
        protected const int XP_HildaHolPfanne = 5;

        #endregion ExperianceConstants

        protected override string NpcName { get; set; } = "Хилдия";

        protected override void AddButtons()
        {
            SpeechInfoButtons.Add(HalloInfo);
            SpeechInfoButtons.Add(WasZuEssenInfo);
            SpeechInfoButtons.Add(BringBeetInfo);
            SpeechInfoButtons.Add(EinkaufenInfo);
            SpeechInfoButtons.Add(PfanneGeholtInfo);
        }

        private void HalloInfo()
        {
            if (HalloInfoCondition() == false)
                return;

            AddContext(DialogId.LogartPro);

            void Action()
            {
                EndDialog();
            }

            Play(Action);
        }

        private bool HalloInfoCondition()
        {
            return PlayerProgress.QuestContainer.GetState(QuestId.LobartCollectBeet) != QuestState.Success;
        }

        private void WasZuEssenInfo()
        {
            if (WasZuEssenInfoCondition() == false)
                return;

            CreateStartSpeechButton(WasZuEssen, DialogId.LogartPro, nameof(WasZuEssenInfo));
        }

        private bool WasZuEssenInfoCondition()
        {
            return PlayerProgress.QuestContainer.GetState(QuestId.LobartCollectBeet) == QuestState.Success
                   && IsNpsKnow("WasZuEssenInfo") == false;
        }

        private void WasZuEssen()
        {
            AddContext(DialogId.LogartPro);

            void Action()
            {
                RestartDialogs();
            }
            Play(Action);
        }

        private void BringBeetInfo()
        {
            if (BringBeetCondition() == false)
                return;

            CreateStartSpeechButton(BringBeet, DialogId.LogartPro, nameof(BringBeetInfo));
        }

        private bool BringBeetCondition()
        {
            return IsNpsKnow("BringBeetInfo") == false
                   && SlotsHandler.CalculateAmount(ItemId.Food_Beet, PlayerProgress.PlayerData.SlotsContainer) >= 20;
        }

        private void BringBeet()
        {
            AddContext(
                true, "Я принес тебе репу...",
                "DIA_Hilda_BringBeet_15_00");
            AddContext(
                false, "Отлично! (смеется) Этого должно хватить, чтобы накормить наших работников до отвала!",
                "DIA_Hilda_BringBeet_17_01");
            AddContext(
                false, "Раз уж ты все равно здесь... Я видела, как мимо прошел странствующий торговец. Это было несколько минут назад.",
                "DIA_Hilda_BringBeet_17_02");

            void Action()
            {
                PlayerProgress.QuestContainer.ChangeState(QuestId.RuebenToHilda, QuestState.Success);
                PlayerProgress.PlayerData.PlayerCharacteristics.IncrementExperience(XP_Ambient);
                DecrementAmount(ItemId.Food_Beet, 20);
                RestartDialogs();
            }
            Play(Action);
        }

        private void EinkaufenInfo()
        {
            if (EinkaufenInfoCondition() == false)
                return;
            CreateStartSpeechButton(Einkaufen, DialogId.LogartPro, nameof(EinkaufenInfo));
        }

        private bool EinkaufenInfoCondition()
        {
            return IsNpsKnow("EinkaufenInfo") == false
                   && IsNpsKnow("BringBeetInfo");
        }

        private void Einkaufen()
        {
            AddContext(
                true, "Дай мне золота, и я схожу к этому торговцу для тебя...",
                "DIA_Hilda_Einkaufen_15_00");
            AddContext(
                false, "А тебе можно доверять? Только попробуй потратить эти деньги на выпивку, слышишь?!",
                "DIA_Hilda_Einkaufen_17_01");

            void Action()
            {
                AddItem(ItemId.Gold, 5);
                PlayerProgress.QuestContainer.ChangeState(QuestId.Hilda_PfanneKaufen, QuestState.Running);
                RestartDialogs();
            }
            Play(Action);
        }

        private void PfanneGeholtInfo()
        {
            if (PfanneGeholtInfoCondition() == false)
                return;

            CreateStartSpeechButton(PfanneGeholt, DialogId.LogartPro, nameof(PfanneGeholtInfo));
        }

        private bool PfanneGeholtInfoCondition()
        {
            return IsNpsKnow("PfanneGeholtInfo") == false
                   && PlayerProgress.QuestContainer.GetState(QuestId.Hilda_PfanneKaufen) == QuestState.Running
                   && SlotsHandler.CalculateAmount(ItemId.Bratpfanne, PlayerProgress.PlayerData.SlotsContainer) > 0;
        }

        private void PfanneGeholt()
        {
            AddContext(true, "Вот твоя сковородка.",
                "DIA_Hilda_PfanneGeholt_15_00");
            AddContext(false, "Отлично. Посмотрим, хорошая ли она...",
                "DIA_Hilda_PfanneGeholt_17_01");

            void Action()
            {
                PlayerProgress.PlayerData.PlayerCharacteristics.IncrementExperience(XP_HildaHolPfanne);
                PlayerProgress.QuestContainer.ChangeState(QuestId.Hilda_PfanneKaufen, QuestState.Success);
                RestartDialogs();
            }
            Play(Action);
        }
    }
}