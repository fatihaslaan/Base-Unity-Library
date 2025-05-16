using System;
using AG.Base.Addressable;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AG.Base.SceneTransition
{
    internal sealed class SceneTransitionModel : AddressableSceneLoader
    {
        internal event Action OnSceneLoadFailed;

        internal void LoadScene(AddressableSceneName sceneName)
        {
            InitiateAddressableOperation(sceneName, OnOperationSucceed, OnOperationFailed);

            void OnOperationSucceed()
            {
                Debug.Log("Scene Load Completed");
                if (GetLoadSceneMode() == LoadSceneMode.Additive)
                {
                    Resources.UnloadUnusedAssets();
                }
            }

            void OnOperationFailed()
            {
                OnSceneLoadFailed?.Invoke();
            }
        }
    }
}