using UnityEngine;
namespace DesignPatterns
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;

                var objs = FindObjectsOfType(typeof(T)) as T[];

                if (objs is { Length: > 0 }) _instance = objs[0];

                if (objs is { Length: > 1 })
                {
                    Console.LogError("Singleton", "There is more than one " + typeof(T).Name + " in the scene.");
                }

                if (_instance != null) return _instance;
                GameObject obj = new GameObject
                {
                    hideFlags = HideFlags.HideAndDontSave
                };
                _instance = obj.AddComponent<T>();
                return _instance;
            }
        }

        public bool CheckAcvtive() => _instance == null ? false : true;


        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }


    public class SingletonPersistent<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (_instance == null)
                    {
                        SetupInstance();
                    }
                    else
                    {
                        string typeName = typeof(T).Name;

                        Console.Log("Singleton", typeName + " instance already created: " +
                            _instance.gameObject.name);
                    }
                }

                return _instance;
            }
        }

        public virtual void Awake()
        {
            RemoveDuplicates();

        }

        private static void SetupInstance()
        {
            // lazy instantiation
            _instance = (T)FindObjectOfType(typeof(T));

            if (_instance == null)
            {
                GameObject gameObj = new GameObject();
                gameObj.name = typeof(T).Name;

                _instance = gameObj.AddComponent<T>();
                DontDestroyOnLoad(gameObj);
            }
        }

        private void RemoveDuplicates()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public bool CheckInstance()
        {
            if (_instance == null)
            {
                Console.LogWarning("Singleton", $"[Singleton] Instance of {typeof(T).Name} is NULL.");
                return false;
            }

            if (_instance.gameObject == null)
            {
                Console.LogWarning("Singleton", $"[Singleton] Instance of {typeof(T).Name} has a NULL GameObject.");
                return false;
            }

            Console.LogWarning("Singleton", $"[Singleton] Instance of {typeof(T).Name} is valid: {_instance.gameObject.name}");
            return true;
        }
    }
}