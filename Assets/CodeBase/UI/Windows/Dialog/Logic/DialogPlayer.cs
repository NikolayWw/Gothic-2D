using CodeBase.Infrastructure.Logic;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Dialog;
using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.UI.Windows.Dialog.Logic
{
    public class DialogPlayer
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IStaticDataService _dataService;
        private readonly DialogAudio _audio;
        private readonly DialogBuilder _dialogBuilder;

        private DialogData _dialogData;

        private int _currentIndex;

        public DialogPlayer(ICoroutineRunner coroutineRunner, DialogBuilder dialogBuilder, IStaticDataService dataService, DialogAudio audio)
        {
            _coroutineRunner = coroutineRunner;
            _dialogBuilder = dialogBuilder;
            _dataService = dataService;
            _audio = audio;
        }

        public void Play(in DialogData data)
        {
            _dialogData = data;
            _currentIndex = 0;
            PlayProcess();
        }

        private void PlayProcess()
        {
            //if (_speechContext.Count > _index)
            //{
            //    DialogData data = _speechContext[_index];
            //    _dialogBuilder.ShowOutputText(data.IsGGFocus ? "Gg" : data.NpcName, data.Dialog);

            //    AudioClip speech = _dataService.ForNpcSpeech(data.AudioName.ToUpper());

            //    if (speech != null)
            //        _audio.Play(speech, PlayProcess);
            //    else
            //        _coroutineRunner.StartCoroutine(DelayToNextPlay());

            //    _index++;
            //}
            //else
            //{
            //    foreach (var action in _onEndPlays)
            //        action?.Invoke();
            //}
        }

        private IEnumerator DelayToNextPlay()
        {
            yield return new WaitForSeconds(_dataService.DialogStaticData.DialogueTimeWithoutSound);
            PlayProcess();
        }
    }
}