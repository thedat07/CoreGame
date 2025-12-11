using UnityEngine;
using DG.Tweening;

namespace Creator
{
    public class SceneDefaultAnimation : SceneAnimation
    {
        #region Enum

        protected enum State
        {
            IDLE,
            SHOW,
            HIDE
        }

        protected enum AnimationType
        {
            None,
            Fade,
            Scale,
            SlideFromBottom,
            SlideFromTop,
            SlideFromLeft,
            SlideFromRight,
            ScaleSpecial
        }

        #endregion

        #region SerializeField

        [SerializeField] private AnimationType m_AnimationType = AnimationType.SlideFromRight;

        [SerializeField] private Ease m_ShowEaseType = Ease.Linear;

        [SerializeField] private Ease m_HideEaseType = Ease.Linear;

        #endregion

        #region Private Variables

        protected Vector2 m_Start;
        protected Vector2 m_End;
        protected RectTransform m_RectTransform;
        protected RectTransform m_CanvasRectTransform;
        protected State m_State = State.IDLE;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (!Application.isPlaying) return;

            m_AnimationDuration = Director.SceneAnimationDuration > GetDataNullDefault()
                ? Director.SceneAnimationDuration
                : 0.25f;
        }

        #endregion

        #region Cached Properties

        private RectTransform RectTransform
        {
            get
            {
                if (m_RectTransform == null)
                    TryGetComponent(out m_RectTransform);
                return m_RectTransform;
            }
        }

        private CanvasGroup CanvasGroup => GetCanvasGroup();

        private RectTransform CanvasRectTransform
        {
            get
            {
                if (m_CanvasRectTransform == null)
                {
                    Transform p = transform.parent;
                    while (p != null)
                    {
                        if (p.TryGetComponent(out Canvas canvas))
                        {
                            p.TryGetComponent(out m_CanvasRectTransform);
                            break;
                        }
                        p = p.parent;
                    }
                }
                return m_CanvasRectTransform;
            }
        }

        #endregion

        #region Public Override Methods

        public override void HideBeforeShowing()
        {
            switch (m_AnimationType)
            {
                case AnimationType.Fade:
                    CanvasGroup.alpha = GetDataNullDefault();
                    break;
                case AnimationType.Scale:
                case AnimationType.ScaleSpecial:
                    RectTransform.localScale = Vector3.zero;
                    break;
                case AnimationType.SlideFromBottom:
                    RectTransform.anchoredPosition = new Vector2(0, -ScreenHeight());
                    break;
                case AnimationType.SlideFromTop:
                    RectTransform.anchoredPosition = new Vector2(0, ScreenHeight());
                    break;
                case AnimationType.SlideFromLeft:
                    RectTransform.anchoredPosition = new Vector2(-ScreenWidth(), RectTransform.anchoredPosition.y);
                    break;
                case AnimationType.SlideFromRight:
                    RectTransform.anchoredPosition = new Vector2(ScreenWidth(), RectTransform.anchoredPosition.y);
                    break;
            }

            Show();
        }

        public override void Show()
        {
            (m_Start, m_End) = GetShowPositions();
            if (m_AnimationType != AnimationType.None)
            {
                m_State = State.SHOW;
                Play();
            }
            else OnShown();
        }

        public override void Hide()
        {
            (m_Start, m_End) = GetHidePositions();
            if (m_AnimationType != AnimationType.None)
            {
                m_State = State.HIDE;
                Play();
            }
            else OnHidden();
        }

        protected override Tween Effect()
        {
            Ease ease = m_State == State.SHOW ? m_ShowEaseType : m_HideEaseType;
            Sequence seq = HelperCreator.DOTweenSequence(gameObject);

            switch (m_AnimationType)
            {
                case AnimationType.Scale:
                    seq.Append(CreateScaleTween(ease));
                    seq.Join(CreateFadeTween(GetAnimationDuration()));
                    break;
                case AnimationType.ScaleSpecial:
                    seq.Append(CreateScaleSpecialTween());
                    seq.Join(CreateFadeTween(0.15f));
                    break;
                case AnimationType.Fade:
                    seq.Append(CreateFadeOnlyTween(ease));
                    break;
                default:
                    seq.Append(CreateMoveTween(ease));
                    seq.Join(CreateFadeTween(GetAnimationDuration()));
                    break;
            }

            if (shield != null)
            {
                seq.Join(shield
                    .DOFade(GetFade(true), GetAnimationDuration())
                    .From(GetFade())
                    .SetEase(Ease.Linear));
            }

            return seq;
        }

        protected override void OnEndAnimation()
        {
            if (m_State == State.SHOW) OnShown();
            else if (m_State == State.HIDE) OnHidden();

            m_State = State.IDLE;
        }

        public override void SetImmediate() => m_AnimationType = AnimationType.None;

        #endregion

        #region Tween Creators

        private Tween CreateFadeTween(float time) =>
            CanvasGroup.DOFade(m_State == State.SHOW ? GetDataDefault() : GetDataNullDefault(), time)
                .From(m_State == State.SHOW ? GetDataNullDefault() : GetDataDefault())
                .SetEase(Ease.Linear);

        private Tween CreateScaleTween(Ease ease)
        {
            if (m_State == State.SHOW)
            {
                RectTransform.localScale = Vector3.zero;
                return HelperCreator.Register(GetAnimationDurationFlash(), () => CreateScaleTween(ease), gameObject);
            }

            return RectTransform
                .DOScale(new Vector3(m_End.x, m_End.y, GetDataDefault()), GetAnimationDuration())
                .From(new Vector3(m_Start.x, m_Start.y, GetDataDefault()))
                .SetEase(ease);
        }

        private Tween CreateScaleSpecialTween() =>
            m_State == State.SHOW ? CreateScaleSpecialShow() : CreateScaleSpecialHide();

        private Tween CreateScaleSpecialShow()
        {
            Vector3[] scales =
            {
                new(0, 0, 0),
                new(1.02f, 1.085f, 1.02f),
                new(1f, 0.975f, 1f),
                new(1f, 1.008f, 1f),
                Vector3.one
            };

            float[] times = { 0f, 0.13f, 0.1f, 0.11f, 0.11f };

            DOTween.Kill(transform);

            var seq = DOTween.Sequence();
            for (int i = 0; i < scales.Length; i++)
                seq.Append(transform.DOScale(scales[i], times[i] * 1.5f).SetEase(Ease.Linear));
            return seq;
        }

        private Tween CreateScaleSpecialHide()
        {
            Vector3[] scales =
            {
                Vector3.one,
                new(1, 1.0378f, 1),
                Vector3.zero
            };

            float[] times = { 0f, 0.07f, 0.08f };

            DOTween.Kill(transform);

            var seq = DOTween.Sequence();
            for (int i = 0; i < scales.Length; i++)
                seq.Append(transform.DOScale(scales[i], times[i] * 1.5f).SetEase(Ease.Linear));
            return seq;
        }

        private Tween CreateMoveTween(Ease ease) =>
            RectTransform.DOAnchorPos(m_End, GetAnimationDuration())
                .From(m_Start)
                .SetEase(ease);

        private Tween CreateFadeOnlyTween(Ease ease)
        {
            if (m_State == State.SHOW)
                return HelperCreator.Register(GetAnimationDurationFlash(), () => CreateFadeOnlyTween(ease), gameObject);

            return CanvasGroup.DOFade(m_End.y, GetAnimationDuration())
                .From(m_Start.x)
                .SetEase(ease);
        }

        #endregion

        #region Helpers

        private (Vector2 start, Vector2 end) GetShowPositions()
        {
            return m_AnimationType switch
            {
                AnimationType.SlideFromBottom => (new Vector2(0, -ScreenHeight()), Vector2.zero),
                AnimationType.SlideFromTop => (new Vector2(0, ScreenHeight()), Vector2.zero),
                AnimationType.SlideFromRight => (new Vector2(ScreenWidth(), RectTransform.anchoredPosition.y), new Vector2(0, RectTransform.anchoredPosition.y)),
                AnimationType.SlideFromLeft => (new Vector2(-ScreenWidth(), RectTransform.anchoredPosition.y), new Vector2(0, RectTransform.anchoredPosition.y)),
                AnimationType.Scale or AnimationType.ScaleSpecial or AnimationType.Fade => (Vector2.zero, Vector2.one),
                _ => (Vector2.zero, Vector2.zero)
            };
        }

        private (Vector2 start, Vector2 end) GetHidePositions()
        {
            return m_AnimationType switch
            {
                AnimationType.SlideFromBottom => (Vector2.zero, new Vector2(0, -ScreenHeight())),
                AnimationType.SlideFromTop => (Vector2.zero, new Vector2(0, ScreenHeight())),
                AnimationType.SlideFromRight => (new Vector2(0, RectTransform.anchoredPosition.y), new Vector2(ScreenWidth(), RectTransform.anchoredPosition.y)),
                AnimationType.SlideFromLeft => (new Vector2(0, RectTransform.anchoredPosition.y), new Vector2(-ScreenWidth(), RectTransform.anchoredPosition.y)),
                AnimationType.Scale or AnimationType.ScaleSpecial or AnimationType.Fade => (Vector2.one, Vector2.zero),
                _ => (Vector2.zero, Vector2.zero)
            };
        }

        private float ScreenHeight() => CanvasRectTransform != null ? CanvasRectTransform.sizeDelta.y : Screen.height;
        private float ScreenWidth() => CanvasRectTransform != null ? CanvasRectTransform.sizeDelta.x : Screen.width;

        private float GetAnimationDuration() => m_AnimationDuration;
        private float GetAnimationDurationFlash() => m_AnimationDuration / 2;

        private float GetFade(bool back = false) =>
            back == false
                ? (m_State == State.SHOW ? GetDataNullDefault() : GetFadeDefault())
                : (m_State == State.SHOW ? GetFadeDefault() : GetDataNullDefault());

        #endregion

        #region Public Methods

        public void PlayShow(bool hide = true)
        {
            Sequence seq = HelperCreator.DOTweenSequence(gameObject);

            seq.Append(CanvasGroup.DOFade(GetDataDefault(), GetAnimationDurationFlash()).SetEase(Ease.Linear));

            if (hide && shield != null)
                seq.Join(shield.DOFade(GetFadeDefault(), GetAnimationDurationFlash()).SetEase(Ease.Linear));

            seq.OnComplete(() => CanvasGroup.blocksRaycasts = true);
        }

        public void PlayHide(bool hide = true)
        {
            CanvasGroup.alpha = GetDataNullDefault();
            CanvasGroup.blocksRaycasts = false;

            if (hide && shield != null)
            {
                Color colorShield = shield.color;
                colorShield.a = GetDataNullDefault();
                shield.color = colorShield;
            }

            m_State = State.HIDE;
        }

        #endregion
    }
}
