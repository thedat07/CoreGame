using UnityEngine;
using UnityEngine.UI;
using UnityUtilities;

namespace Creator
{
    public interface IController
    {
        string SceneName();
    }

    public abstract class Controller : MonoBehaviour, IController
    {
        protected GameObject m_Shield;

        [Header("Info")]
        [SerializeField] protected Canvas m_Canvas;
        [SerializeField] protected Camera m_Camera;

        [Header("Setting")]
        public bool FullScreen;
        public bool HasShield = true;
        public bool UseCameraUI = true;

        [Header("Effect")]
        public SceneAnimation Animation;

        /// <summary>
        /// Each scene must has an unique scene name.
        /// </summary>
        /// <returns>The name.</returns>
        public abstract string SceneName();

        /// <summary>
        /// This event is raised right after this view becomes active after a call of LoadScene() or ReloadScene() or Popup() of SceneManager.
        /// Same OnEnable but included the data which is transfered from the previous view. (Raised after Awake() and OnEnable())
        /// </summary>
        /// <param name="data">Data.</param>
        public virtual void OnActive(object data)
        {
        }

        /// <summary>
        /// This event is raised right after the above view is hidden.
        /// </summary>
        public virtual void OnReFocus()
        {
        }

        /// <summary>
        /// This event is raised right after this view appears and finishes its show-animation.
        /// </summary>
        public virtual void OnShown()
        {
        }

        /// <summary>
        /// This event is raised right after this view finishes its hide-animation and disappears.
        /// </summary>
        public virtual void OnHidden()
        {
        }

        /// <summary>
        /// This event is raised right after player pushs the ESC button on keyboard or Back button on android devices.
        /// You should assign this method to OnClick event of your Close Buttons.
        /// </summary>
        public virtual void OnKeyBack()
        {
            Director.PopScene();
        }


        public Director.Data Data
        {
            get;
            set;
        }

        public Canvas Canvas
        {
            get
            {
                return m_Canvas;
            }
            set
            {
                m_Canvas = value;
            }
        }

        public Camera Camera
        {
            get
            {
                return m_Camera;
            }
            set
            {
                m_Camera = value;
            }
        }

        public virtual void Show()
        {
            if (Animation)
                Animation.StartShow();
        }

        public virtual void Hide()
        {
            if (Animation)
                Animation.StartHide();
        }

        public virtual void CreateShield()
        {
            if (m_Shield == null && m_Canvas.sortingOrder > 0 && HasShield)
            {
                m_Shield = Instantiate(Resources.Load<GameObject>("Shield"));

                m_Shield.name = "Shield";

                Transform t = m_Shield.transform;

                SettingTransform();

                SettingRectTransform();

                if (m_Shield)
                {
                    m_Shield.TryGetComponent<Image>(out Animation.shield);
                }

                void SettingRectTransform()
                {
                    if (t.TryGetComponent<RectTransform>(out RectTransform rt))
                    {
                        rt.anchorMin = Vector2.zero;
                        rt.anchorMax = Vector2.one;
                        rt.pivot = new Vector2(0.5f, 0.5f);
                        rt.offsetMax = new Vector2(2, 2);
                        rt.offsetMin = new Vector2(-2, -2);
                    }
                }

                void SettingTransform()
                {
                    t.SetParent(m_Canvas.transform);
                    t.SetSiblingIndex(0);
                    t.SetLocalScale(1, 1, 1);
                    t.SetLocalPosition(t.localPosition.x, t.localPosition.y, 0);
                }
            }
        }

        public void SetupCanvas(int sortingOrder)
        {
            if (m_Canvas == null)
            {
                m_Canvas = transform.GetComponentInChildren<Canvas>(true);
            }

            if (UseCameraUI)
            {
                m_Canvas.sortingOrder = sortingOrder;
                m_Canvas.worldCamera = Director.Object.UICamera;
            }

            GetCanvasScaler().EditCanvasScaler();
        }

        public void HidePopup(bool hideShield)
        {
            (Animation as SceneDefaultAnimation).PlayHide(hideShield);
        }

        public void ShowPopup(bool hideShield)
        {
            (Animation as SceneDefaultAnimation).PlayShow(hideShield);
        }

        private RectTransform m_RectTransform;

        private CanvasScaler m_CanvasScaler;

        private CanvasGroup m_CanvasGroup;

        public RectTransform GetRect()
        {
            if (m_RectTransform == null)
            {
                TryGetComponent<RectTransform>(out m_RectTransform);
            }
            return m_RectTransform;
        }

        public CanvasGroup GetCanvasGroup()
        {
            if (m_CanvasGroup == null)
            {
                Animation.TryGetComponent<CanvasGroup>(out m_CanvasGroup);
            }
            return m_CanvasGroup;
        }

        public CanvasScaler GetCanvasScaler()
        {
            if (m_CanvasScaler == null)
            {
                m_Canvas.TryGetComponent<CanvasScaler>(out m_CanvasScaler);
            }
            return m_CanvasScaler;
        }
    }
}