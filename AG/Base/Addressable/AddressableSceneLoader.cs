using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace AG.Base.Addressable
{
    //Loads Addressable Scene
    public abstract class AddressableSceneLoader : BaseAddressableLoader<AddressableSceneName, SceneInstance>
    {
        protected sealed override AsyncOperationHandle CreateOperation(AddressableSceneName sceneName, Action<SceneInstance> onOperationSucceed, Action onOperationFailed)
        {
            return AddressableManager.LoadAddressableSceneAsync(sceneName, onOperationSucceed, onOperationFailed, GetLoadSceneMode(), IsActivateSceneOnLoad());
        }

        protected override void ReleaseOperationMethod(ref AsyncOperationHandle operation)
        {
            if (GetLoadSceneMode() == LoadSceneMode.Single)
            {
                //In Single Mode Releasing Previous Scene Done Automatically
                base.ReleaseOperationMethod(ref operation);
            }
            else
            {
                //Release Previous Scene If Current Mode Is Additive
                AddressableManager.ReleaseScene(ref operation);
            }
        }

        protected virtual LoadSceneMode GetLoadSceneMode()
        {
            return LoadSceneMode.Single;
        }

        protected virtual bool IsActivateSceneOnLoad()
        {
            return true;
        }
    }
}