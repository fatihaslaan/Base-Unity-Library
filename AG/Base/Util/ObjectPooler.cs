using System;
using System.Collections.Generic;
using UnityEngine;

namespace AG.Base.Util
{
    public class ObjectPooler<T> where T : MonoBehaviour
    {
        private Queue<T> _pool = new Queue<T>();
        private Func<T> _createObjectFunction;

        //Create Your Pool With Unique Object Creation Function
        public ObjectPooler(Func<T> createObjectFunction, int startingSize = 10)
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

        public T GetObject()
        {
            if (_pool.Count > 0)
            {
                T obj = _pool.Dequeue();
                obj.gameObject.SetActive(true);
                return obj;
            }

            return _createObjectFunction();
        }

        public void ReturnObject(T obj)
        {
            obj.gameObject.SetActive(false);
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