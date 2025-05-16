using UnityEngine.ResourceManagement.AsyncOperations;
using System;

namespace AG.Base.Addressable
{
    //Load Single Asset
    public class AddressableLoader<TResult> : BaseAddressableLoader<object, TResult>
    {
        protected sealed override AsyncOperationHandle CreateOperation(object addressableReference, Action<TResult> onOperationSucceed, Action onOperationFailed)
        {
            return AddressableManager.LoadAddressableAssetAsync(addressableReference, onOperationSucceed, onOperationFailed);
        }
    }
}