using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using System;

namespace AG.Base.Addressable
{
    //Loads Multiple Assets By AddressableLabelNames
    public class AddressablesLoader<TResult> : BaseAddressableLoader<AddressableLabelName, IList<TResult>>
    {
        protected sealed override AsyncOperationHandle CreateOperation(AddressableLabelName labelName, Action<IList<TResult>> onOperationSucceed, Action onOperationFailed)
        {
            return AddressableManager.LoadAddressableAssetsAsync(labelName, onOperationSucceed, onOperationFailed);
        }
    }
}