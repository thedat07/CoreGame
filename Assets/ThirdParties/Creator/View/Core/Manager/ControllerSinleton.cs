using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;

namespace Creator
{
    public class SingletonController<T> : Controller where T : Component
    {
        public override string SceneName()
        {
            return "";
        }

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
                    Console.LogError("Controller", "There is more than one " + typeof(T).Name + " in the scene.");
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

        public static bool IsInstance() => _instance != null;

        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }
}