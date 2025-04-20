using UnityEngine;

namespace AG.Base.Addressable
{
    //Gets Desired Type From Instantiated Addressable GameObject
    public abstract class AddressableTypeInstantiator<TResult> : AddressableInstantiator where TResult : MonoBehaviour
    {
        protected TResult operationTResult = default;

        protected override GameObject FinalizeOperationResult(GameObject result)
        {
            if (result.TryGetComponent(out TResult tempResult))
            {
                operationTResult = tempResult;
            }
            else if (AllowComponentAddition())
            {
                operationTResult = result.AddComponent<TResult>();
                Debug.Log("No Component Found On Instantiated Result, Added One");
            }
            else
            {
                return null;
            }
            return result;
        }

        //Decide Whether To Add Component On TryGetComponent Fails
        protected virtual bool AllowComponentAddition()
        {
            return false;
        }
    }
}