using System.Collections;
using UnityEngine;

namespace CodeBase.Infrastructure.Logic
{
    public class LoadCurtain
    {
        private const string CurtainPath = "Infrastucture/LoadinCurtain";

        private CanvasGroup _loadCurtain;

        private readonly CanvasGroup _curtainPrefab;
        private readonly ICoroutineRunner _coroutine;
        private bool _isCurtainEnable;

        public LoadCurtain(ICoroutineRunner coroutine)
        {
            _coroutine = coroutine;
            _curtainPrefab = Resources.Load<GameObject>(CurtainPath).GetComponent<CanvasGroup>();
        }

        public void Show()
        {
            if (_isCurtainEnable)
                return;

            _loadCurtain = Object.Instantiate(_curtainPrefab);
            _isCurtainEnable = true;
        }

        public void Hide()
        {
            if (_isCurtainEnable == false)
                return;

            _coroutine.StartCoroutine(HideCurtain());
            _isCurtainEnable = false;
        }

        private IEnumerator HideCurtain()
        {
            do
            {
                _loadCurtain.alpha -= 0.04f;
                yield return null;
            } while (_loadCurtain.alpha > 0.0f);

            Object.Destroy(_loadCurtain.gameObject);
        }
    }
}