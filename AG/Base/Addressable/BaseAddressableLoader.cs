using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using System;

namespace AG.Base.Addressable
{
    public abstract class BaseAddressableLoader<TKey, TResult> : MonoBehaviour
    {
        [NonSerialized]
        protected TResult operationResult = default;

        private AsyncOperationHandle _currentAsyncOperation = default;

        protected void InitiateAddressableOperation(TKey addressableReference, Action onOperationSucceed, Action onOperationFailed)
        {
            SetCurrentOperation(CreateOperation(addressableReference, OperationSucceed, OperationFailed));

            void OperationSucceed(TResult result)
            {
                operationResult = FinalizeOperationResult(result);
                //Check Finalize Status
                if (!operationResult.Equals(default))
                {
                    onOperationSucceed?.Invoke();
                }
                else
                {
                    OperationFailed();
                }
            }

            void OperationFailed()
            {
                //TODO: Call Info Panel Util
                ReleaseOperation();
                onOperationFailed?.Invoke();
            }

            void SetCurrentOperation(AsyncOperationHandle operation)
            {
                //Release Previous Operation
                ReleaseOperation();
                _currentAsyncOperation = operation;
            }
        }

        //Call This Method On Child For Releasing
        protected void ReleaseOperation()
        {
            ReleaseOperationMethod(_currentAsyncOperation);
            _currentAsyncOperation = default;
        }

        //Return Addressable Load Method
        protected abstract AsyncOperationHandle CreateOperation(TKey addressableReference, Action<TResult> onOperationSucceed, Action onOperationFailed);

        //Addressable Release Method
        protected virtual void ReleaseOperationMethod(AsyncOperationHandle operation)
        {
            //Make Sure That No References Left And Release Operation From Memory
            ClearOperationResult();
            AddressableManager.ReleaseAsset(operation);
        }

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