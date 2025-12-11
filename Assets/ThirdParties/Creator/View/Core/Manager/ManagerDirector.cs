using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

namespace Creator
{
    public class ManagerDirector : ManagerBase
    {
        public static void RunScene(string sceneName, object data = null, string log = "")
        {
            if (Application.CanStreamedLevelBeLoaded(sceneName))
            {
                m_DataQueue.Enqueue(new Data(data, sceneName, null, null));
                m_MainSceneName = sceneName;
                Object.FadeOutScene();
            }
        }

        public static void PushScene(string sceneName, object data = null, Callback onShown = null, Callback onHidden = null, bool hasShield = true, string log = "")
        {
            if (Application.CanStreamedLevelBeLoaded(sceneName))
            {
                m_DataQueue.Enqueue(new Data(data, sceneName, onShown, onHidden, hasShield));
                m_SceneNow = sceneName;
                m_TimeScene = Time.time;
                Object.ShieldOn();
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            }
        }

        public static void ReplaceScene(string sceneName, object data = null, Callback onShown = null, Callback onHidden = null, string log = "")
        {
            if (!Application.CanStreamedLevelBeLoaded(sceneName))
                return;

            var currentController = m_ControllerStack.First();
            currentController.HidePopup(false);
            onHidden += () => { currentController.ShowPopup(false); };

            m_DataQueue.Enqueue(new Data(data, sceneName, onShown, onHidden, false));
            Object.ShieldOn();
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }

        public static void PopScene()
        {
            if (m_ControllerStack.Count > 1)
            {
                ActivatePreviousController(true);
                HideController(true);
            }

            if (m_ControllerStack.Count > 0)
            {
                Object.ShieldOn();
                m_ControllerStack.Peek().Hide();
            }
        }

        public static Controller GetRunningScene()
        {
            return m_ControllerStack.First();
        }

        public static void Pause()
        {
            Time.timeScale = 0f;
        }

        public static void Resume()
        {
            Time.timeScale = 1f;
        }

        public static void PopToRootScene()
        {
            if (m_ControllerStack.Count > 1)
            {
                for (int i = 1; i < m_ControllerStack.Count; i++)
                {
                    RemovePreviousController(m_ControllerStack.Peek());
                }
            }

            PopScene();
        }

        public static Stack<Controller> GetSceneStack()
        {
            return new Stack<Controller>(m_ControllerStack);
        }

        protected static void ActivatePreviousController(bool active)
        {
            ActivatePreviousController(m_ControllerStack.Peek(), active);
        }

        protected static void HideController(bool active)
        {
            HideController(m_ControllerStack.Peek(), active);
        }

        protected static void ActivatePreviousController(Controller controller, bool active)
        {
            Stack<Controller> temp = new Stack<Controller>();

            while (m_ControllerStack.Count > 0)
            {
                var top = m_ControllerStack.Pop();
                temp.Push(top);

                if (top == controller && m_ControllerStack.Count > 0)
                {
                    var previousController = m_ControllerStack.Peek();
                    previousController.gameObject.SetActive(active);
                    break;
                }
            }

            while (temp.Count > 0)
            {
                m_ControllerStack.Push(temp.Pop());
            }
        }

        protected static void HideController(Controller controller, bool active)
        {
            Stack<Controller> temp = new Stack<Controller>();

            while (m_ControllerStack.Count > 0)
            {
                var top = m_ControllerStack.Pop();
                temp.Push(top);

                if (top == controller && m_ControllerStack.Count > 0)
                {
                    var previousController = m_ControllerStack.Peek();
                    if (previousController.Animation.TryGetComponent<CanvasGroup>(out var canvasGroup))
                    {
                        canvasGroup.blocksRaycasts = active;
                    }
                    break;
                }
            }

            while (temp.Count > 0)
            {
                m_ControllerStack.Push(temp.Pop());
            }
        }

        protected static void Unload()
        {
            if (m_ControllerStack.Count > 0)
            {
                Unload(m_ControllerStack.Pop());
            }
        }

        protected static void Unload(Controller controller)
        {
            if (controller != null && controller.Data != null && controller.Data.scene != null)
            {
                SceneManager.UnloadSceneAsync(controller.Data.scene);
            }
        }

        protected static void RemovePreviousController(Controller controller)
        {
            Stack<Controller> temp = new Stack<Controller>();

            while (m_ControllerStack.Count > 0)
            {
                var top = m_ControllerStack.Pop();
                temp.Push(top);

                if (top == controller && m_ControllerStack.Count > 0)
                {
                    var previousController = m_ControllerStack.Pop();
                    Unload(previousController);
                    break;
                }
            }

            while (temp.Count > 0)
            {
                m_ControllerStack.Push(temp.Pop());
            }
        }

        protected static Controller GetController(Scene scene)
        {
            var roots = scene.GetRootGameObjects();
            for (int i = 0; i < roots.Length; i++)
            {
                if (roots[i].TryGetComponent<Controller>(out var component))
                {
                    return component;
                }
            }
            return null;
        }
    }
}
