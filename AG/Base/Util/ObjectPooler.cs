using System;
using System.Collections.Generic;
using UnityEngine;

namespace AG.Base.Util
{
    public class ObjectPooler<TPoolObject> where TPoolObject : MonoBehaviour, IPoolObject
    {
        private Queue<TPoolObject> _pool = new Queue<TPoolObject>();
        private Func<TPoolObject> _createObjectFunction;

        private int fillPoolCount = 5;

        //Create Your Pool With Unique Object Creation Function
        public ObjectPooler(Func<TPoolObject> createObjectFunction, int startingSize = 10)
        {
            _createObjectFunction = createObjectFunction;
            AddToPool(startingSize);
        }

        public void AddToPool(int size)
        {
            for (int i = 0; i < size; i++)
            {
                ReturnObject(_createObjectFunction());
            }
        }

        public TPoolObject GetObject(bool activateOnGet = true, Vector3 position = default, Vector3 rotation = default, Transform parent = null)
        {
            if (_pool.Count > 0)
            {
                TPoolObject obj = _pool.Dequeue();

                obj.transform.position = position;
                obj.transform.eulerAngles = rotation;

                obj.transform.SetParent(parent);
                obj.transform.localScale = Vector3.one;

                obj.gameObject.SetActive(activateOnGet);

                obj.OnObjectGet();
                
                return obj;
            }
            AddToPool(fillPoolCount);

            return GetObject(activateOnGet, position, rotation, parent);
        }

        public void ReturnObject(TPoolObject obj)
        {
            obj.gameObject.SetActive(false);
            obj.OnObjectReturn();
            _pool.Enqueue(obj);
        }

        public void ClearPool()
        {
            while (_pool.Count > 0)
            {
                UnityEngine.Object.Destroy(_pool.Dequeue());
            }
        }
    }
}