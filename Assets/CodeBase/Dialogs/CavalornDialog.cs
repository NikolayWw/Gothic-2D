using CodeBase.Data.PlayerProgress.Player.Quests;
using CodeBase.StaticData.Items;
using CodeBase.StaticData.Npc;

namespace CodeBase.Dialogs
{
    public class CavalornDialog : BaseDialog
    {
        protected override string NpcName { get; set; } = "Кавалорн";

        protected override void AddButtons()
        {
            SpeechInfoButtons.Add(HALLOInfo);
            SpeechInfoButtons.Add(WASMACHSTDUInfo);
            SpeechInfoButtons.Add(HELFENInfo);
            SpeechInfoButtons.Add(AUSRUESTUNGInfo);
            SpeechInfoButtons.Add(BragoKilledInfo);
            SpeechInfoButtons.Add(TeachInfo);
        }

        private void HALLOInfo()
        {
            if (HALLOInfoCondition() == false)
                return;
            CreateStartSpeechButton(HALLO, "У тебя проблемы?", "HALLOInfo");
        }

        private bool HALLOInfoCondition()
        {
            return IsNpsKnow("HALLOInfo") == false;
        }

        private void HALLO()
        {
            AddContext(
                true, "Проблемы?",
                "DIA_Addon_Cavalorn_HALLO_15_00");
            AddContext(
                false, "(хитро) Погоди минутку. Я тебя знаю. Ты тот парень, что постоянно клянчил у меня стрелы в Долине Рудников.?",
                "DIA_Addon_Cavalorn_HALLO_08_02");

            void Action()
            {
                ClearInputs();
                CreateStartSpeechButton(HALLO_Ja, "Тебя зовут Кавалорн, верно", "HALLO");
                CreateStartSpeechButton(HALLO_weissNicht, "Что-то не припоминаю...", "HALLO");
            }
            Play(Action);
        }

        private void HALLO_weissNicht()
        {
            AddContext(true, "Что-то не припоминаю...",
                "DIA_Addon_Cavalorn_HALLO_weissNicht_15_00");
            AddContext(false, "Ну как же. Еще в моей хижине около Старого Лагеря я учил тебя, как стрелять из лука и незаметно передвигаться. Теперь вспоминаешь?",
                "DIA_Addon_Cavalorn_HALLO_weissNicht_08_01");
            void Action()
            {
                RemoveInputButton("Что-то не припоминаю...");
            }
            Play(Action);
        }

        private void HALLO_Ja()
        {
            AddContext(true, "Тебя зовут Кавалорн, верно?",
            "DIA_Addon_Cavalorn_HALLO_Ja_15_00");
            AddContext(false, "Куда ты направляешься?",
                "DIA_Addon_Cavalorn_HALLO_Ja_08_02");

            void Action()
            {
                ClearInputs();
                CreateStartSpeechButton(HALLO_Stadt, "В город.", "HALLO_Ja");
            }
            Play(Action);
        }

        private void HALLO_Stadt()
        {
            AddContext(true, "В город",
                "DIA_Addon_Cavalorn_HALLO_Stadt_15_00");

            AddContext(false, "У тебя могут возникнуть сложности со стражей. Они уже не пускают каждого прохожего, весь район кишит бандитами.",
                "DIA_Addon_Cavalorn_HALLO_Stadt_08_02");

            void Action()
            {
                RestartDialogs();
            }
            Play(Action);
        }

        private void WASMACHSTDUInfo()
        {
            if (WASMACHSTDUInfoCondition() == false)
                return;

            CreateStartSpeechButton(WASMACHSTDU, "Что ты делаешь здесь?", "WASMACHSTDUInfo");
        }

        private bool WASMACHSTDUInfoCondition()
        {
            return IsNpsKnow("WASMACHSTDUInfo") == false
                && IsNpsKnow("HALLOInfo");
        }

        private void WASMACHSTDU()
        {
            AddContext(true, "Что ты здесь делаешь?",
                "DIA_Addon_Cavalorn_WASMACHSTDU_15_00");
            AddContext(false, "Сижу на месте. Если бы не эти чертовы бандиты, меня бы здесь не было.",
                "DIA_Addon_Cavalorn_WASMACHSTDU_08_01");

            Play(RestartDialogs);
        }

        private void HELFENInfo()
        {
            if (HELFENInfoCondition() == false)
                return;

            CreateStartSpeechButton(HELFEN, "Могу я помочь тебе с бандитами", "HELFENInfo");
        }

        private bool HELFENInfoCondition()
        {
            return IsNpsKnow("HELFENInfo") == false
                   && IsNpsKnow("WASMACHSTDUInfo");
        }

        private void HELFEN()
        {
            AddContext(true, "Могу я помочь тебе с бандитами?",
                "DIA_Addon_Cavalorn_HELFEN_15_00");
         
            AddContext(false, "Так, слушай. Вниз по этой дороге располагается одна из тех грязных дыр, где прячутся бандиты.",
                "DIA_Addon_Cavalorn_HELFEN_08_03");

            void Action()
            {
                PlayerProgress.QuestContainer.ChangeState(QuestId.CavalornKillBrago, QuestState.Running);
                RestartDialogs();
            }
            Play(Action);
        }

        private void AUSRUESTUNGInfo()
        {
            if (AUSRUESTUNGInfoCondition() == false)
                return;
            CreateStartSpeechButton(AUSRUESTUNG, "Мне нужна экипировка получше.", "AUSRUESTUNGInfo");
        }

        private bool AUSRUESTUNGInfoCondition()
        {
            return IsNpsKnow("AUSRUESTUNGInfo") == false
                && IsNpsKnow("HELFENInfo");
        }

        private void AUSRUESTUNG()
        {
            AddContext(true, "Мне нужна экипировка получше.",
                "DIA_Addon_Cavalorn_AUSRUESTUNG_15_00");
            AddContext(false, "Я могу дать тебе волчий нож. Этого пока хватит?",
                "DIA_Addon_Cavalorn_AUSRUESTUNG_08_02");
            AddContext(true, "Ты называешь это ножом?",
                "DIA_Addon_Cavalorn_AUSRUESTUNG_15_03");

            void Action()
            {
                AddItem(ItemId.MeleeWeapon_SwordSmall, 1);
                RestartDialogs();
            }
            Play(Action);
        }

        private void BragoKilledInfo()
        {
            if (BragoKilledInfoCondition() == false)
                return;
            CreateStartSpeechButton(BragoKilled, "С бандитами покончено.", "BragoKilledInfo");
        }

        private bool BragoKilledInfoCondition()
        {
            return IsNpsKnow("BragoKilledInfo") == false
                   && IsNpsKnow("HELFENInfo")
                   && PlayerProgress.KillData.Npc.Contains(NpcId.Bandit_Cavalorn1)
                   && PlayerProgress.KillData.Npc.Contains(NpcId.Bandit_Cavalorn2);
        }

        private void BragoKilled()
        {
            AddContext(true, "С бандитами покончено.",
                "DIA_Addon_Cavalorn_PCKilledBrago_15_00");
            AddContext(false, "Готово. Ха. Им не следовало связываться со мной.",
                "DIA_Addon_Cavalorn_BragoKilled_08_00");

            void Action()
            {
                PlayerProgress.QuestContainer.ChangeState(QuestId.CavalornKillBrago, QuestState.Success);
                PlayerProgress.PlayerData.PlayerCharacteristics.IncrementExperience(XP_Addon_Cavalorn_KillBrago);
                RestartDialogs();
            }
            Play(Action);
        }

        private void TeachInfo()
        {
            if (TeachInfoCondition() == false)
                return;
            CreateStartSpeechButton(Teach, "Я хочу стать сильнее.", "TeachInfo");
        }

        private bool TeachInfoCondition()
        {
            return IsNpsKnow("HELFENInfo");
        }

        private void Teach()
        {
            void TryAddStrength(int strength, int decrementLp)
            {
                if (PlayerProgress.PlayerData.PlayerCharacteristics.LP >= strength)
                    AddStrength(strength, decrementLp);
                else
                    NeedMoreLp();
            }
            void AddStrength(int strength, int decrementLp)
            {
                ShowUpStrength(strength);
                PlayerProgress.PlayerData.PlayerCharacteristics.ChangeCharacteristics(0, 0, strength);
                PlayerProgress.PlayerData.PlayerCharacteristics.ChangeCharacteristics(0, 0, 0, -decrementLp);
            }
            void Action()
            {
                ClearInputs();

                CreateStartSpeechButton(() => TryAddStrength(1, 1), "Увеличить на 1", "Teach", false);
                CreateStartSpeechButton(() => TryAddStrength(5, 5), "Увеличить на 5", "Teach", false);

                CreateStartSpeechButton(RestartDialogs, "Назад", "Teach");
            }
            Play(Action);
        }

        private void NeedMoreLp()
        {
            AddContext(false, "У тебя недостаточно опыта.",
                "B_Harad_NotEnoughGold_12_00");

            void Action()
            {
                Teach();
            }
            Play(Action);
        }
    }
}