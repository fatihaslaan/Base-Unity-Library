using System;
using AG.Base.Addressable;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AG.Base.SceneTransition
{
    internal sealed class SceneTransitionModel : AddressableSceneLoader
    {
        internal event Action OnSceneLoadFailed;

        internal void LoadScene(AddressableSceneNames sceneName, Action onSceneLoadStart)
        {
            onSceneLoadStart?.Invoke();
            InitiateAddressableOperation(sceneName);
        }

        protected override void OnOperationFailed()
        {
            OnSceneLoadFailed?.Invoke();
        }

        protected override void OnOperationSucceed()
        {
            Debug.Log("Scene Load Completed");
            if (GetLoadSceneMode() == LoadSceneMode.Additive)
            {
                Resources.UnloadUnusedAssets();
            }
        }
    }
}