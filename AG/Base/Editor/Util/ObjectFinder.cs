using System;
using UnityEngine;

namespace AG.Base.Editor.Util
{
    public static class ObjectFinder
    {
        /// --------------------------------------
        /// For Better Onvalidate Reference Sets
        /// --------------------------------------
        
        private static void FindObjectLogic<T>(ref T objectToFind, Func<T> findObjectFunction) where T : Component
        {
            if (objectToFind == null)
            {
                objectToFind = findObjectFunction();
                if (objectToFind == null)
                {
                    Debug.Log("Failed To Set Reference " + objectToFind.GetType());
                }
            }
        }

        public static void FindObjectInSceneWithType<T>(ref T objectToFind) where T : Component
        {
            FindObjectLogic(ref objectToFind, UnityEngine.Object.FindAnyObjectByType<T>);
        }

        public static void FindObjectInChilderenWithType<T>(ref T objectToFind, Transform transform) where T : Component
        {
            FindObjectLogic(ref objectToFind, transform.GetComponentInChildren<T>);
        }

        public static void FindObjectInChilderenWithName<T>(ref T objectToFind, Transform transform, string name) where T : Component
        {
            if (objectToFind != null) return;

            Transform tempTransform = transform.Find(name);
            if (tempTransform)
            {
                objectToFind = tempTransform.GetComponent<T>();
                if (objectToFind == null)
                {
                    Debug.LogError("Failed To Set Reference On " + transform.name);
                }
            }
            else
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    FindObjectInChilderenWithName(ref objectToFind, transform.GetChild(i), name);
                    if (objectToFind != null) return;
                }
            }
        }
    }
}