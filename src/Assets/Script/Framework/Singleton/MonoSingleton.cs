using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Singleton
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                    throw new ArgumentNullException(typeof(T).FullName, "The mono singleton object is not in the scene or being destroyed.");
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            InitializeSingleton();
        }
        
        protected void InitializeSingleton()
        {
            if (_instance == null)
                _instance = this as T; 
            else
            {
                Debug.LogWarning($"Multiple singleton object {typeof(T)} detected. Duplicate one will be destroyed.");
                Destroy(gameObject);
            }
        }
    }
}

