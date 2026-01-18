using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using YNL.Utilities.Extensions;
using UnityTimer;

namespace Creator
{
    public class Director : ManagerDirector
    {
        protected static string m_LoadingSceneName;
        protected static Controller m_LoadingController;
        private static bool initialized;

        public static string LoadingSceneName
        {
            set
            {
                m_LoadingSceneName = value;
                SceneManager.LoadScene(m_LoadingSceneName, LoadSceneMode.Additive);
            }
            get
            {
                return m_LoadingSceneName;
            }
        }

        public static bool HasLoading() => m_LoadingController != null;

        static Director()
        {
            // CHỈ gán dữ liệu nhẹ, KHÔNG Unity API
            SceneAnimationDuration = 0.25f;

            Creator.Director.LoadingSceneName = DLoading.SCENE_NAME;
        }

        public static void Init()
        {
            if (initialized)
                return;

            initialized = true;

            SceneManager.sceneLoaded += OnSceneLoaded;

            var go = GameObject.Instantiate(
                Resources.Load<GameObject>("ManagerObject")
            );

            Object = go.GetComponent<ManagerObject>();
            Object.gameObject.name = "ManagerObject";
        }

        #region Loading
        public static void LoadingAnimation(bool active)
        {
            if (m_LoadingController != null)
            {
                if (active)
                {
                    (m_LoadingController as ILoading).ShowLoading();
                }
                else
                {
                    (m_LoadingController as ILoading).HideLoading();
                }
            }
        }
        #endregion

        #region Controller
        public static void OnShown(Controller controller)
        {
            if (controller.FullScreen && m_ControllerStack.Count > 1)
            {
                ActivatePreviousController(controller, false);
            }

            Timer.Register(Director.SceneAnimationDuration, () =>
            {
                controller.OnShown();
                if (controller.Data != null && controller.Data.onShown != null)
                {
                    controller.Data.onShown();
                }
            }, autoDestroyOwner: controller);

            Object.ShieldOff();
        }

        public static void StartShow(Controller controller)
        {
            HideController(controller, false);
        }

        public static void OnHidden(Controller controller)
        {
            controller.OnHidden();
            controller.Data?.onHidden?.Invoke();

            if (m_ControllerStack.Count > 1)
            {
                Unload();
            }

            if (m_ControllerStack.Count > 0)
            {
                m_ControllerStack.Peek().OnReFocus();
            }

            Object.ShieldOff();
        }


        public static void OnFadedIn()
        {
            m_MainController.OnShown();
        }

        public static void OnFadedOut()
        {
            if (m_MainController != null)
            {
                m_MainController.OnHidden();
            }
            LoadrAsync(m_MainSceneName);
        }

        static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Get Controller
            var controller = GetController(scene);

            if (controller == null)
                return;

            if (controller.SceneName() == LoadingSceneName)
            {
                SettingController(ref m_LoadingController, 90);
                return;
            }

            void SettingController(ref Controller controllerRef, int sortingOrder)
            {
                controllerRef = controller;
                controllerRef.HasShield = false;
                controllerRef.FullScreen = false;
                controllerRef.UseCameraUI = true;
                controller.SetupCanvas(sortingOrder);
                controllerRef.gameObject.SetActive(false);
                GameObject.DontDestroyOnLoad(controllerRef.gameObject);
            }

            // Single Mode automatically destroy all scenes, so we have to clear the stack.
            if (mode == LoadSceneMode.Single)
            {
                m_ControllerStack.Clear();
            }

            // Unload resources and collect GC.
            Resources.UnloadUnusedAssets();
            System.GC.Collect();

            // Get Data
            if (m_DataQueue.Count == 0)
            {
                m_DataQueue.Enqueue(new Data(null, scene.name, null, null));
            }

            Data data = m_DataQueue.Dequeue();
            while (data.sceneName != scene.name && m_DataQueue.Count > 0)
            {
                data = m_DataQueue.Dequeue();
            }

            if (data == null)
            {
                data = new Data(null, scene.name, null, null);
            }

            data.scene = scene;

            // Push the current scene to the stack.
            m_ControllerStack.Push(controller);

            // Setup controller
            controller.Data = data;
            controller.HasShield = data.hasShield;
            controller.SetupCanvas(m_ControllerStack.Count - 1);
            controller.OnActive(data.data);
            controller.CreateShield();
            // Animation
            if (m_ControllerStack.Count == 1)
            {
                MCamera.SetupBaseAndOverlayCameras(controller.Camera, Object.UICamera);

                // Main Scene
                m_MainController = controller;
                if (string.IsNullOrEmpty(m_MainSceneName))
                {
                    m_MainSceneName = scene.name;
                }

                // Fade
                Object.FadeInScene();
            }
            else
            {
                // Popup Scene
                controller.Show();
            }
        }
        #endregion

        #region LoadingScene

        static void LoadrAsync(string sceneName)
        {
            if (HasLoading())
            {
                Director.LoadingAnimation(true);
                m_LoadingController.StopAllCoroutines();
                m_LoadingController.StartCoroutine(LoadYourAsyncScene(sceneName));
            }
            else
            {
                SceneManager.LoadScene(m_MainSceneName, LoadSceneMode.Single);
            }
        }

        static IEnumerator LoadYourAsyncScene(string sceneName)
        {
            yield return new WaitForSeconds(0.5f);

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            while (!asyncLoad.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
        }
        #endregion
    }
}