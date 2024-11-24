using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace unity_base
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        static T instance;
        [SerializeField] bool dontDestroyOnLoad;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
                        Debug.LogWarningFormat("[SINGLETON] The class {0} could not be found in the scene!", typeof(T));
                    }
                }
                return instance;
            }
        }

        public virtual void Awake()
        {
            if (instance == null)
            {
                if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
                instance = this as T;
            }
            else if (instance != this)
            {
                Debug.LogWarningFormat("[SINGLETON] There is more than one instance of class {0} in the scene!", typeof(T));
                Destroy(this.gameObject);
            }
        }

        private void OnDestroy()
        {
            instance = null;
        }
    }
}
