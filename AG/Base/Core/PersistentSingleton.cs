using UnityEngine;

namespace AG.Base.Core
{
    public class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
    {
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(transform.root.gameObject);
        }
    }
}