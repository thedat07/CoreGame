using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

namespace Creator
{
    public class ManagerBase
    {
        public class Data
        {
            public object data;
            public Callback onShown;
            public Callback onHidden;
            public Scene scene;
            public string sceneName;
            public bool hasShield;

            public Data(object data, string sceneName, Callback onShown, Callback onHidden, bool hasShield = true)
            {
                this.data = data;
                this.sceneName = sceneName;
                this.onShown = onShown;
                this.onHidden = onHidden;
                this.hasShield = hasShield;
            }
        }

        public delegate void Callback();

        protected static Stack<Controller> m_ControllerStack = new Stack<Controller>();

        protected static Queue<Data> m_DataQueue = new Queue<Data>();

        protected static float m_TimeScene;

        protected static string m_SceneNow;

        public static int stackCount
        {
            get
            {
                return m_ControllerStack.Count;
            }
        }

        public static Controller MainController
        {
            get
            {
                return m_MainController;
            }
        }

        public static float SceneAnimationDuration
        {
            get;
            set;
        }

        public static ManagerObject Object
        {
            get;
            protected set;
        }

        protected static string m_MainSceneName;

        protected static Controller m_MainController;
    }
}
