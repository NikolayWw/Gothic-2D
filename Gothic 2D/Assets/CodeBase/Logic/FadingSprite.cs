#region

using System.Collections;
using UnityEngine;

#endregion

namespace CodeBase.Logic
{
    public class FadingSprite : MonoBehaviour
    {
        [SerializeField] [Range(0f, 1f)] private float _fadingForce;
        [SerializeField] private PlayerTriggerReporter _playerTrigger;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Coroutine _fadingEnumerator;
        private Coroutine _unFadingEnumerator;

        private void Awake()
        {
            _playerTrigger.OnTriggeredEnter += Fading;
            _playerTrigger.OnTriggeredExit += UnFading;
        }

        private void Fading()
        {
            if (_unFadingEnumerator != null)
                StopCoroutine(_unFadingEnumerator);
            _fadingEnumerator = StartCoroutine(FadingProcess());
        }

        private void UnFading()
        {
            StopCoroutine(_fadingEnumerator);
            _unFadingEnumerator = StartCoroutine(UnFadingProcess());
        }

        private IEnumerator FadingProcess()
        {
            var color = _spriteRenderer.color;
            while (color.a > _fadingForce)
            {
                color.a -= Time.deltaTime;
                _spriteRenderer.color = color;
                yield return null;
            }

            color.a = _fadingForce;
            _spriteRenderer.color = color;
        }

        private IEnumerator UnFadingProcess()
        {
            var color = _spriteRenderer.color;
            while (color.a < 1f)
            {
                color.a += Time.deltaTime;
                _spriteRenderer.color = color;

                yield return null;
            }

            color.a = 1;
            _spriteRenderer.color = color;
        }
    }
}