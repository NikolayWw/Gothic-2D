using CodeBase.Data.PlayerProgress.Player.Quests;
using CodeBase.StaticData.Items;

namespace CodeBase.Dialogs
{
    public class LobartDialog : BaseDialog
    {
        protected override string NpcName { get; set; } = "Лобарт";

        protected override void AddButtons()
        {
            InfoButtons.Add(HalloInfo);
            InfoButtons.Add(WorkNOWInfo);
            InfoButtons.Add(RuebenRunningInfo);
            InfoButtons.Add(MoreWorkInfo);
        }

        private void HalloInfo()
        {
            if (HalloInfoCondition() == false)
                return;

            AddContext(false,
                "Что ты тут ошиваешься на моей ферме?",
                "DIA_Lobart_Hallo_05_00");

            AddContext(false,
                "Ты на чьей стороне? На стороне восставших фермеров или на стороне короля?",
                "DIA_Lobart_Hallo_05_01");

            void Action()
            {
                ClearInputs();
                CreateInput(Hallo_ForTheKing, "Я за короля!", "HalloInfo");
                CreateInput(Hallo_ForThePeasants, "Я с крестьянами!", "HalloInfo");
            }

            Play(Action);
        }

        private bool HalloInfoCondition()
        {
            return IsNpsKnow("HalloInfo") == false;
        }

        private void Hallo_ForThePeasants()
        {
            AddContext(true,
                "Я с крестьянами!",
                "DIA_Lobart_Hallo_ForThePeasants_15_00");

            AddContext(false,
                "Ха! Этот чертов смутьян Онар сведет нас всех в могилу!",
                "DIA_Lobart_Hallo_ForThePeasants_05_01");

            void Action()
            {
                RestartDialogs();
            }
            Play(Action);
        }

        private void Hallo_ForTheKing()
        {
            AddContext(true, "Я за короля", "DIA_Lobart_Hallo_ForTheKing_15_00");

            void Action()
            {
                RestartDialogs();
            }

            Play(Action);
        }

        private void WorkNOWInfo()
        {
            if (WorkNOWInfoCondition() == false)
                return;

            CreateInput(WorkNOW, "Я ищу работу", "WorkNOWInfo");
        }

        private bool WorkNOWInfoCondition()
        {
            return IsNpsKnow("WorkNOWInfo") == false && IsNpsKnow("HalloInfo");
        }

        private void WorkNOW()
        {
            AddContext(true,
                "Я ищу работу",
                "DIA_Lobart_WorkNOW_15_00");

            AddContext(false,
                "Работа следующая - небольшое поле репы за амбаром нужно собрать",
                "DIA_Lobart_WorkNOW_05_07");

            void Action()
            {
                ClearInputs();
                CreateInput(WorkNOW_WannaFoolMe, "Я должен дергать репу? Ты, должно быть, шутишь!", "WorkNOW");
                CreateInput(WorkNOW_Ok, "Хорошо...", "WorkNOW");
            }

            Play(Action);
        }

        private void WorkNOW_Ok()
        {
            AddContext(true,
                "Хорошо...",
                "DIA_Lobart_WorkNOW_Ok_15_00");

            void Action()
            {
                PlayerProgress.QuestContainer.ChangeState(QuestId.LobartCollectBeet, QuestState.Running);
                RestartDialogs();
            }

            Play(Action);
        }

        private void WorkNOW_WannaFoolMe()
        {
            AddContext(
                true,
                "Я должен дергать репу? Ты, должно быть, шутишь!",
                 "DIA_Lobart_WorkNOW_WannaFoolMe_15_00");
            AddContext(
                false,
                "Сейчас у меня нет другой работы для тебя.",
                "DIA_Lobart_WorkNOW_WannaFoolMe_05_03."
            );

            void Action()
            {
                PlayerProgress.QuestContainer.ChangeState(QuestId.LobartCollectBeet, QuestState.Running);
                RestartDialogs();
            }

            Play(Action);
        }

        private void RuebenRunningInfo()
        {
            if (RuebenRunningCondition() == false)
                return;

            CreateInput(RuebenRunning, "Вот твоя репа", "RuebenRunningInfo");
        }

        private bool RuebenRunningCondition()
        {
            return PlayerProgress.QuestContainer.GetState(QuestId.LobartCollectBeet) == QuestState.Running
                 && IsNpsKnow("WorkNOW")
                 && SlotsHandler.CalculateAmount(ItemId.Food_Beet, PlayerProgress.PlayerData.SlotsContainer) >= 20
                && IsNpsKnow("RuebenRunningInfo") == false;
        }

        private void RuebenRunning()
        {
            AddContext(
                true,
                "Вот твоя репа!",
                "DIA_Lobart_RuebenRunning_15_00");
            AddContext(
                false,
                "Отнеси ее моей жене в дом и скажи ей, чтобы она приготовила ее.",
                "DIA_Lobart_RuebenRunning_05_02");
            AddContext(
                false,
                "Я могу дат тебе 5 золотых монет",
                "DIA_Lobart_RuebenRunning_05_04");

            void Action()
            {
                AddItem(ItemId.Gold, 5);
                PlayerProgress.PlayerData.PlayerCharacteristics.IncrementExperience(XP_LobartHolRueben);
                PlayerProgress.QuestContainer.ChangeState(QuestId.LobartCollectBeet, QuestState.Success);
                PlayerProgress.QuestContainer.ChangeState(QuestId.RuebenToHilda, QuestState.Running);
                RestartDialogs();
            }
            Play(Action);
        }

        private void MoreWorkInfo()
        {
            if (MoreWorkCondition() == false)
                return;

            CreateInput(MoreWork, "У тебя есть еще какая-нибудь работа для меня?", "MoreWorkInfo");
        }

        private bool MoreWorkCondition()
        {
            return IsNpsKnow("RuebenRunningInfo") &&
                   PlayerProgress.QuestContainer.GetState(QuestId.LobartCollectBeet) == QuestState.Running ||
                   PlayerProgress.QuestContainer.GetState(QuestId.LobartCollectBeet) == QuestState.Success;
        }

        private void MoreWork()
        {
            AddContext(true,
                "У тебя есть еще какая-нибудь работа для меня",
                "DIA_Lobart_MoreWork_15_00");

            void Action()
            {
                RestartDialogs();
            }
            Play(Action);
        }
    }
}