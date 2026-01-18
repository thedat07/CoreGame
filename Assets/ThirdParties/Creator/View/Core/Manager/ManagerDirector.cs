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
            if (m_ControllerStack.Count <= 1)
            {
                return;
            }

            ActivatePreviousController(true);
            HideController(true);

            Object.ShieldOn();
            m_ControllerStack.Peek().Hide();
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
            if (m_ControllerStack == null || m_ControllerStack.Count == 0)
                return;

            while (m_ControllerStack.Count > 1)
            {
                PopTopControllerImmediate();
            }

            var rootController = m_ControllerStack.Peek();
            if (rootController == null)
                return;

            if (rootController.Animation
                .TryGetComponent<CanvasGroup>(out var canvasGroup))
            {
                canvasGroup.blocksRaycasts = true;
            }
        }


        protected static void PopTopControllerImmediate()
        {
            if (m_ControllerStack.Count == 0)
                return;

            var controller = m_ControllerStack.Pop();

            RemovePendingDataForScene(controller.SceneName());

            SceneManager.UnloadSceneAsync(controller.Data.scene);

            if (m_ControllerStack.Count > 0)
            {
                m_ControllerStack.Peek().OnReFocus();
            }
        }

        static void RemovePendingDataForScene(string sceneName)
        {
            if (m_DataQueue.Count == 0)
                return;

            var temp = new Queue<Data>();

            while (m_DataQueue.Count > 0)
            {
                var data = m_DataQueue.Dequeue();

                if (data.sceneName == sceneName)
                    continue;

                temp.Enqueue(data);
            }

            m_DataQueue = temp;
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
            if (m_ControllerStack.Count == 0)
                return;


            if (m_ControllerStack.Peek() != controller)
                return;

            var removed = m_ControllerStack.Pop();
            Unload(removed);
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