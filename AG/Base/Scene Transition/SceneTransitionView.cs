using System;
using AG.Base.Addressable;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AG.Base.SceneTransition
{
    internal sealed class SceneTransitionView : AddressableTypeInstantiator<SceneTransitionLoadingScreen>
    {
        internal event Action OnTransitionPrefabLoadFailed;

        [SerializeField] private AssetReference sceneTransitionPrefab;

        internal void ShowLoading()
        {
            InitiateAddressableOperation(sceneTransitionPrefab);
        }

        internal void HideLoading()
        {
            if (operationResult)
            {
                operationResult.StopTransitionAnimation(ReleaseOperation);
            }
        }

        protected override void OnOperationFailed()
        {
            OnTransitionPrefabLoadFailed?.Invoke();
        }

        protected override void OnOperationSucceed()
        {
            operationResult.PlayTransitionAnimation();
        }

        protected override Transform GetInstantiatedObjectParent()
        {
            return transform;
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if(sceneTransitionPrefab == null)
            {
                Debug.Log("Scene Transition Prefab Is Not Setted");
            }
        }

#endif
    }
}
