using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using System;

namespace AG.Base.Addressable
{
    //Loads Multiple Assets By AddressableLabelNames
    public abstract class AddressablesLoader<TResult> : BaseAddressableLoader<AddressableLabelNames, IList<TResult>>
    {
        protected sealed override AsyncOperationHandle CreateOperation(AddressableLabelNames addressableReference, Action<IList<TResult>> onOperationSucceed, Action onOperationFailed)
        {
            return AddressableManager.LoadAddressableAssetsAsync(addressableReference, onOperationSucceed, onOperationFailed);
        }
    }
}