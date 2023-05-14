using CodeBase.Data.PlayerProgress;
using CodeBase.Data.PlayerProgress.Npc;
using CodeBase.Infrastructure.Logic;
using CodeBase.Inventory;
using CodeBase.Services.GameFactory;
using CodeBase.Services.LogicFactory;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Dialog;
using CodeBase.StaticData.Items;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Window;
using CodeBase.UI.Windows.Dialog;
using CodeBase.UI.Windows.Dialog.Logic;
using CodeBase.UI.Windows.GamePlayMessage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Dialogs
{
    public abstract class BaseDialog : MonoBehaviour, ICoroutineRunner
    {
        [field: SerializeField] protected OpenBox Shop { get; private set; }
        protected abstract string NpcName { get; set; }
        protected PlayerProgress PlayerProgress { get; private set; }
        protected IGameFactory GameFactory { get; private set; }
        protected IStaticDataService DataService { get; private set; }
        protected InventorySlotsHandler SlotsHandler { get; private set; }

        protected List<Action> SpeechInfoButtons { get; } = new List<Action>();

        private List<string> _dialogNpsKnows;
        private DialogBuilder _dialogBuilder;
        private IUIFactory _uiFactory;
        private IWindowService _windowService;
        private bool _isInDialog;
        private CloseGameWindows _closeGameWindows;
        private readonly List<DialogId> _addedSpeechIds = new List<DialogId>();

        public void Construct(IUIFactory uiFactory, NpcData npcData, IPersistentProgressService persistentProgressService, IGameFactory gameFactory, IStaticDataService dataService, IWindowService windowService, ILogicFactoryService logicFactory)
        {
            _uiFactory = uiFactory;
            DataService = dataService;
            GameFactory = gameFactory;
            _windowService = windowService;
            SlotsHandler = logicFactory.InventorySlotsHandler;
            _closeGameWindows = logicFactory.CloseGameWindows;

            _dialogNpsKnows = npcData.DialogNpcKnows;
            PlayerProgress = persistentProgressService.PlayerProgress;
        }

        private void OnDestroy()
        {
            Clean();
        }

        public async void StartDialog()
        {
            if (_isInDialog)
                return;

            _closeGameWindows.Close();
            _isInDialog = true;

            AddButtons();
            _dialogBuilder = await _uiFactory.CreateDialogWindow();
            _dialogBuilder.OnDialogWindowClosed += Clean;
            await _dialogBuilder.StartDialog(_dialogNpsKnows, SpeechInfoButtons);
        }

        private void Clean()
        {
            if (_dialogBuilder != null)
                _dialogBuilder.OnDialogWindowClosed -= Clean;
            _isInDialog = false;
        }

        protected void EndDialog()
        {
            _dialogBuilder.EndDialog();
            _isInDialog = false;
        }

        protected void ShowGiveItem(ItemId itemId, int amount)
        {
            if (_windowService.TryGetWindow(WindowId.GameplayMessage, out GamePlayMessageWindow window) == false)
                return;

            window.ShowMessage(GamePlayMessageId.Item, $"Получено {DataService.ForItem(itemId).InspectorName} {amount}x");
        }

        protected void ShowUpStrength(int strength)
        {
            if (_windowService.TryGetWindow(WindowId.GameplayMessage, out GamePlayMessageWindow window) == false)
                return;

            window.ShowMessage(GamePlayMessageId.Strength, $"Сила +{strength}");
        }

        protected abstract void AddButtons();

        protected void RemoveInputButton(string name) =>
            _dialogBuilder.RemoveButton(name);

        protected void AddContext(params DialogId[] speechIds)
        {
            foreach (var speechId in speechIds)
            {
                _addedSpeechIds.Add(speechId);
            }
        }

        protected async void RestartDialogs() =>
            await _dialogBuilder.ReStartDialogs();

        protected async void CreateStartSpeechButton(Action onStartDialog, DialogId dialogId, string npsKnows, bool clearAfterClick = true)
        {
            await _dialogBuilder.CreateInput(OnStartDialog, dialogId, npsKnows, clearAfterClick);
            void OnStartDialog()
            {
                _addedSpeechIds.Clear();
                onStartDialog?.Invoke();
            }
        }

        protected void ClearInputs() =>
            _dialogBuilder.ClearInputs();

        protected void Play(Action onEndPlaySpeech)
        {
            DialogData data = new DialogData(NpcName, _addedSpeechIds, onEndPlaySpeech);
            _dialogBuilder.Play(data);
        }

        protected async void AddItem(ItemId id, int amount)
        {
            ShowGiveItem(id, amount);
            await SlotsHandler.AddOrDropItem(id, amount, PlayerProgress.PlayerData.SlotsContainer);
        }

        protected void DecrementAmount(ItemId id, int amount) =>
            SlotsHandler.DecrementAmount(id, ref amount, PlayerProgress.PlayerData.SlotsContainer);

        protected bool IsNpsKnow(string action) =>
            _dialogBuilder.IsNpsKnow(action);
    }
}