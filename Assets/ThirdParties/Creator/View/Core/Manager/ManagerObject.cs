using UnityEngine;
using UnityEngine.EventSystems;

namespace Creator
{
    public class ManagerObject : BaseSS
    {
        enum State
        {
            SHIELD_OFF,
            SHIELD_ON,
            SHIELD_FADE_IN,
            SHIELD_FADE_OUT,
            SCENE_LOADING
        }

        [SerializeField] Camera m_UiCamera;

        State m_State;

        public Camera UICamera => m_UiCamera;

        public void ShieldOff()
        {
            if (m_State == State.SHIELD_ON)
            {
                m_State = State.SHIELD_OFF;
            }
        }

        public void ShieldOn()
        {
            if (m_State == State.SHIELD_OFF)
            {
                m_State = State.SHIELD_ON;
            }
        }

        // Scene gradually appear
        public void FadeInScene()
        {
            if (this != null)
            {
                OnFadedIn();
            }
        }

        // Scene gradually disappear
        public void FadeOutScene()
        {
            if (this != null)
            {
                OnFadedOut();
            }
        }

        public void OnFadedIn()
        {
            if (this != null)
            {
                m_State = State.SHIELD_OFF;
                Creator.Director.OnFadedIn();
            }
        }

        public void OnFadedOut()
        {
            m_State = State.SCENE_LOADING;
            Creator.Director.OnFadedOut();
        }

        protected override void OnEndAnimation()
        {
            switch (m_State)
            {
                case State.SHIELD_FADE_IN:
                    OnFadedIn();
                    break;
                case State.SHIELD_FADE_OUT:
                    OnFadedOut();
                    break;
            }
        }

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }


        void Start()
        {
            if (EventSystem.current != null)
            {
                int defaultValue = EventSystem.current.pixelDragThreshold;
                EventSystem.current.pixelDragThreshold = Mathf.Max(defaultValue, (int)(defaultValue * Screen.dpi / 160f));
            }

            ShieldOff();
        }
    }
}