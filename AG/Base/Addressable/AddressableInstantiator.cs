using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using System;

namespace AG.Base.Addressable
{
    //Instantiates Addressable GameObject
    public class AddressableInstantiator : BaseAddressableLoader<object, GameObject>
    {
        //Sealed For Preventing Wrong Usage
        protected sealed override AsyncOperationHandle CreateOperation(object addressableReference, Action<GameObject> onOperationSucceed, Action onOperationFailed)
        {
            return AddressableManager.InstantiateAddressableAssetAsync(addressableReference, onOperationSucceed, onOperationFailed, GetInstantiatedObjectParent());
        }

        protected override void ReleaseOperationMethod(AsyncOperationHandle operation)
        {
            AddressableManager.ReleaseInstance(operation);
        }

        protected virtual Transform GetInstantiatedObjectParent()
        {
            return null;
        }
    }
}