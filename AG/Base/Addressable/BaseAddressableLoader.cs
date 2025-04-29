using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using System;

namespace AG.Base.Addressable
{
    public abstract class BaseAddressableLoader<TKey, TResult> : MonoBehaviour
    {
        protected TResult operationResult = default;

        private AsyncOperationHandle _currentAsyncOperation = default;

        protected void InitiateAddressableOperation(TKey addressableReference)
        {
            SetCurrentOperation(CreateOperation(addressableReference, OperationSucceed, OperationFailed));
        }

        private void SetCurrentOperation(AsyncOperationHandle operation)
        {
            //Release Previous Operation
            ReleaseOperation();
            _currentAsyncOperation = operation;
        }

        private void OperationSucceed(TResult result)
        {
            operationResult = FinalizeOperationResult(result);
            //Check Finalize Status
            if (!operationResult.Equals(default))
            {
                OnOperationSucceed();
            }
            else
            {
                OperationFailed();
            }
        }

        private void OperationFailed()
        {
            //TODO: Call Info Panel Util
            ReleaseOperation();
            OnOperationFailed();
        }

        //Call This Method On Child For Releasing
        protected void ReleaseOperation()
        {
            ReleaseOperationMethod(ref _currentAsyncOperation);
        }

        //Addressable Release Method
        protected virtual void ReleaseOperationMethod(ref AsyncOperationHandle operation)
        {
            //Make Sure That No References Left And Release Operation From Memory
            ClearOperationResult();
            AddressableManager.ReleaseAsset(ref operation);
        }

        //Return Addressable Load Method
        protected abstract AsyncOperationHandle CreateOperation(TKey addressableReference, Action<TResult> onOperationSucceed, Action onOperationFailed);

        protected abstract void OnOperationSucceed();

        protected abstract void OnOperationFailed();

        //Clear All References Before Releasing
        protected virtual void ClearOperationResult()
        {
            operationResult = default;
        }

        //Decide What To Do With Loaded Addressable Before Setting It As Result
        protected virtual TResult FinalizeOperationResult(TResult result)
        {
            return result;
        }

        //Release Current Operation OnDestroy For Preventing OnSucceed Call And NullReference Error
        protected virtual void OnDestroy()
        {
            ReleaseOperation();
        }
    }
}