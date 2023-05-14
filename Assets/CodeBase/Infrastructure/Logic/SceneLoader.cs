using CodeBase.Data;
using System;
using System.Collections;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.Logic
{
    public class SceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void Load(string name, Action onLoaded = null)
        {
            _coroutineRunner.StartCoroutine(LoadInitialScene(name, onLoaded));
        }

        private IEnumerator LoadInitialScene(string name, Action onLoaded)
        {
            if (SceneManager.GetActiveScene().name != GameConstants.InitialScene)
            {
                var async = SceneManager.LoadSceneAsync(GameConstants.InitialScene);

                do
                {
                    yield return null;
                } while (async.isDone == false);

                _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded));
                yield break;
            }

            _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded));
        }

        private IEnumerator LoadScene(string name, Action onLoaded)
        {
            if (name == SceneManager.GetActiveScene().name)
            {
                onLoaded?.Invoke();
                yield break;
            }

            AsyncOperationHandle<SceneInstance> waitLoadScene = Addressables.LoadSceneAsync(name);

            do
            {
                yield return null;
            } while (waitLoadScene.IsDone == false);

            onLoaded?.Invoke();
        }
    }
}