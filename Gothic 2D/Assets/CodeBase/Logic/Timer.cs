using CodeBase.Infrastructure.Logic;
using System.Collections;
using UnityEngine;

namespace CodeBase.Logic
{
    public class Timer
    {
        private readonly ICoroutineRunner _coroutineRunner;
        public bool IsTimerElapsed { get; private set; } = true;

        public Timer(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void StartTimer(float timeToElapse)
        {
            _coroutineRunner.StartCoroutine(TimerProcess(timeToElapse));
        }

        private IEnumerator TimerProcess(float timeToElapse)
        {
            IsTimerElapsed = false;
            yield return new WaitForSeconds(timeToElapse);
            IsTimerElapsed = true;
        }
    }
}