using CodeBase.Infrastructure.Logic;
using CodeBase.Services.StaticData;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Windows.Dialog.Logic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Windows.Dialog
{
    public class DialogBuilder : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private GameObject _outputWindow;
        [SerializeField] private GameObject _inputWindow;
        [SerializeField] private TMP_Text _nameOutputText;
        [SerializeField] private TMP_Text _dialogOutputText;
        [SerializeField] private DialogAudio _dialogAudio;
        public Action OnDialogWindowClosed;
        private IStaticDataService _staticDataService;

        private DialogPlayer _dialogPlayer;
        private List<string> _npsKnows;

        private List<Action> _infoInputButtons = new List<Action>();
        private readonly Dictionary<string, DialogButton> _inputButtons = new Dictionary<string, DialogButton>();

        private readonly List<DialogData> _contextData = new List<DialogData>();
        private IUIFactory _uiFactory;

        public void Construct(IStaticDataService staticDataService, IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
            _staticDataService = staticDataService;
            _dialogPlayer = new DialogPlayer(this, this, _staticDataService, _dialogAudio);
        }

        private void Awake()
        {
            HideOutputWindow();
        }

        public async Task StartDialog(List<string> npsKnows, List<Action> infoButtons)
        {
            _npsKnows = npsKnows;
            _infoInputButtons = infoButtons;
            await ReStartDialogs();
        }

        public void EndDialog()
        {
            _infoInputButtons.Clear();
            OnDialogWindowClosed?.Invoke();
            Destroy(gameObject);
        }

        public async Task ReStartDialogs()
        {
            ClearInputs();
            _infoInputButtons.ForEach(x => x?.Invoke());
            await CreateInput(EndDialog, "Exit", "Exit");
        }

        public void AddContent(in DialogData data)
        {
            _contextData.Add(data);
        }

        public void Play(Action action)
        {
            HidInputWindow();
            ShowOutputWindow();
            _dialogPlayer.Play(_contextData, Action);

            void Action()
            {
                action?.Invoke();
                HideOutputWindow();
                ShowInputWindow();
                _contextData.Clear();
            }
        }

        public async Task CreateInput(Action action, string buttonName, string npsKnows, bool clearAfterClick = true)
        {
            DialogButton button = await _uiFactory.CreateDialogButton(_inputWindow.transform, buttonName);
            button.Button.onClick.AddListener(ActionAndNpsKnows);

            _inputButtons[buttonName] = button;

            void ActionAndNpsKnows()
            {
                action?.Invoke();
                if (clearAfterClick)
                    RemoveButton(buttonName);
                if (npsKnows != null && IsNpsKnow(npsKnows) == false)
                    _npsKnows.Add(npsKnows);
            }
        }

        public void ShowOutputText(string npcName, string dialog)
        {
            ShowOutputWindow();
            _nameOutputText.text = npcName;
            _dialogOutputText.text = dialog;
        }

        public bool IsNpsKnow(string action)
        {
            return _npsKnows.Contains(action);
        }

        public void ClearInputs()
        {
            foreach (var value in _inputButtons.Values)
                value.Close();
            _inputButtons.Clear();
        }

        public void RemoveButton(string buttonName)
        {
            if (_inputButtons.TryGetValue(buttonName, out DialogButton button))
            {
                button.Close();
                _inputButtons.Remove(buttonName);
            }
        }

        private void ShowInputWindow() =>
            _inputWindow.SetActive(true);

        private void HidInputWindow() =>
            _inputWindow.SetActive(false);

        private void ShowOutputWindow() =>
            _outputWindow.SetActive(true);

        private void HideOutputWindow() =>
            _outputWindow.SetActive(false);
    }
}