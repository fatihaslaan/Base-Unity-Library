using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace AG.Base.Addressable
{
    //Manages All Addressable Calls
    public static class AddressableManager
    {
        //Track Ongoing Operations For;
        //-Showing On Loads
        //-Preventing NullReference Errors On Destroyed Object's Addressable Call
        public static int OngoingOperationCount { get { return _ongoingOperations.Count; } }

        private static List<AsyncOperationHandle> _ongoingOperations = new();

        private static AsyncOperationHandle<T> LoadAddressableLogic<T>(Func<AsyncOperationHandle<T>> operationFunc, string errorMessage, Action<T> onSuccess, Action onFail)
        {
            AsyncOperationHandle<T> asyncOperation = operationFunc();

            //Start Tracking Handle
            _ongoingOperations.Add(asyncOperation);

            asyncOperation.Completed += operation =>
            {
                if(_ongoingOperations.Contains(asyncOperation))
                {
                    _ongoingOperations.Remove(asyncOperation);
                    if (operation.Status == AsyncOperationStatus.Failed)
                    {
                        Debug.LogError($"{errorMessage} {operation.OperationException}");
                        onFail?.Invoke();
                    }
                    else
                    {
                        onSuccess?.Invoke(operation.Result);
                    }
                }
                else
                {
                    Debug.Log("Addressable Load Completed After Object Is Destroyed");
                }
            };

            return asyncOperation;
        }

        public static AsyncOperationHandle<T> LoadAddressableAssetAsync<T>(object addressableReference, Action<T> onSuccess, Action onFail)
        {
            if (IsReferenceNull(addressableReference, onFail))
            {
                return default;
            }
            return LoadAddressableLogic(() => Addressables.LoadAssetAsync<T>(addressableReference), $"Error Loading Asset: {addressableReference}", onSuccess, onFail);
        }

        public static AsyncOperationHandle<IList<T>> LoadAddressableAssetsAsync<T>(AddressableLabelName labelName, Action<IList<T>> onSuccess, Action onFail)
        {
            return LoadAddressableLogic(() => Addressables.LoadAssetsAsync<T>(labelName.ToString()), $"Error Loading Assets With Label: {labelName}", onSuccess, onFail);
        }

        public static AsyncOperationHandle<GameObject> InstantiateAddressableAssetAsync(object addressableReference, Action<GameObject> onSuccess, Action onFail, Transform parent = null)
        {
            if (IsReferenceNull(addressableReference, onFail))
            {
                return default;
            }
            return LoadAddressableLogic(() => Addressables.InstantiateAsync(addressableReference, parent), $"Error Instantiating Asset: {addressableReference}", onSuccess, onFail);
        }

        public static AsyncOperationHandle<SceneInstance> LoadAddressableSceneAsync(AddressableSceneName scenName, Action<SceneInstance> onSuccess, Action onFail, LoadSceneMode loadMode = LoadSceneMode.Single, bool activateOnLoad = true)
        {
            return LoadAddressableLogic(() => Addressables.LoadSceneAsync(scenName.ToString(), loadMode, activateOnLoad), $"Error Loading Scene: {scenName}", onSuccess, onFail);
        }

        private static bool IsReferenceNull(object reference, Action onReferenceNull)
        {
            if (reference == null)
            {
                Debug.LogError("Addressable Reference Is Null! Cannot Load The Asset.");
                onReferenceNull?.Invoke();
                return true;
            }
            return false;
        }

        #region Release

        public static void ReleaseAsset(AsyncOperationHandle operation)
        {
            Release(operation, Addressables.Release);
        }

        public static void ReleaseInstance(AsyncOperationHandle operation)
        {
            Release(operation, op => Addressables.ReleaseInstance(op));
        }

        public static void ReleaseScene(AsyncOperationHandle sceneOperation, bool autoReleaseHandle = true)
        {
            Release(sceneOperation, op => Addressables.UnloadSceneAsync(op, autoReleaseHandle));
        }

        private static void Release(AsyncOperationHandle operation, Action<AsyncOperationHandle> releaseFunction)
        {
            if (operation.IsValid())
            {
                if (_ongoingOperations.Contains(operation))
                {
                    _ongoingOperations.Remove(operation);
                }
                releaseFunction.Invoke(operation);
            }
        }

        #endregion
    }
}