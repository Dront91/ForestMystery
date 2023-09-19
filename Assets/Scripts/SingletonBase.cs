using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MysteryForest
{
    [DisallowMultipleComponent]
    public abstract class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        [Header("Singleton")]
        [SerializeField] private bool DoNotDestroyOnLoad;

        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("MonoSingleton: object of type already exists, instance will be destroyed = " + typeof(T).Name);
                Destroy(this);
                return;
            }

            Instance = this as T;

            if (DoNotDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }
    }
}
