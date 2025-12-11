using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using UnityUtilities;

namespace UnityUtilities
{
    public static class DOTweenExtensions
    {
        /// <summary>
        /// Reset lại tất cả tween trên Transform (rewind và kill tween đang chạy).
        /// </summary>
        public static void DoResetDefault(this Transform target)
        {
            target.DORewind();
            target.DOKill();
        }

        /// <summary>
        /// Tween số trên TextMeshProUGUI từ giá trị hiện tại đến value,
        /// có thể chuyển đổi số sang dạng rút gọn (abbreviated string).
        /// </summary>
        public static Tween DoValue(this TextMeshProUGUI target, int value, bool convert = true)
        {
            if (!int.TryParse(target.text, out int start))
                start = 0;

            if (!convert)
                return DOVirtual.Int(start, value, 0.5f, x => { target.text = x.ToString(); });
            else
                return DOVirtual.Int(start, value, 0.5f, x => { target.text = x.ToAbbreviatedString(); });
        }

        /// <summary>
        /// Tween số trên TextMeshProUGUI từ 'from' đến 'value' trong khoảng thời gian 'time',
        /// có thể chuyển đổi sang dạng rút gọn (abbreviated string).
        /// </summary>
        public static Tween DoValue(this TextMeshProUGUI target, int from, int value, bool convert = true, float time = 0.5f)
        {
            if (!convert)
                return DOVirtual.Int(from, value, time, x => { target.text = x.ToString(); });
            else
                return DOVirtual.Int(from, value, time, x => { target.text = x.ToAbbreviatedString(); });
        }

        /// <summary>
        /// Tween số trên Text từ startValue đến endValue trong duration,
        /// dùng format để format chuỗi hiển thị.
        /// </summary>
        public static Tweener DoValue(this Text target, int startValue, int endValue, float duration, string format = "{0}")
        {
            return DOVirtual.Int(startValue, endValue, duration, x =>
            {
                target.text = string.Format(format, x);
            });
        }

        /// <summary>
        /// Tween số trên Text từ startValue đến endValue trong duration,
        /// hiển thị số dạng rút gọn (abbreviate).
        /// </summary>
        public static Tweener DoValueAbbreviateNumber(this Text target, int startValue, int endValue, float duration, string format = "{0}")
        {
            return DOVirtual.Int(startValue, endValue, duration, x =>
            {
                target.text = string.Format(format, x.ToAbbreviatedString());
            });
        }

        /// <summary>
        /// Tween số trên Text từ giá trị hiện tại (nếu hợp lệ) hoặc 0 đến endValue trong duration,
        /// dùng format để format chuỗi hiển thị.
        /// </summary>
        public static Tweener DoValue(this Text target, int endValue, float duration, string format = "{0}")
        {
            if (!int.TryParse(target.text, out int startValue))
                startValue = 0;

            return DOVirtual.Int(startValue, endValue, duration, x =>
            {
                target.text = string.Format(format, x);
            });
        }

        /// <summary>
        /// Hiệu ứng nhấp nháy đổi màu đỏ rồi trở về màu mặc định nhiều lần cho TextMeshProUGUI.
        /// </summary>
        public static Tweener FlashTextRedMultipleTimes(this TextMeshProUGUI textMeshPro, Color flashColor, Color defaultColor)
        {
            textMeshPro.DOKill();
            textMeshPro.color = flashColor;

            return textMeshPro.DOColor(defaultColor, 0.5f)
                .SetEase(Ease.OutFlash)
                .SetLink(textMeshPro.gameObject, LinkBehaviour.KillOnDestroy);
        }

        /// <summary>
        /// Tween giá trị thanh Slider đến value trong 0.2s,
        /// nếu value = 0 thì ẩn thanh fill.
        /// </summary>
        public static Tweener DoProgressValue(this Slider target, float value)
        {
            target.DOKill();

            if (value == 0)
            {
                if (target.fillRect != null)
                    target.fillRect.gameObject.SetActive(false);
                return null;
            }
            else
            {
                if (target.fillRect != null)
                    target.fillRect.gameObject.SetActive(true);

                float clampedValue = Mathf.Clamp(value, target.maxValue / 200f, target.maxValue);
                return target.DOValue(clampedValue, 0.2f);
            }
        }

        /// <summary>
        /// Tween vị trí local của Transform về Vector3.zero trong duration giây.
        /// </summary>
        public static Tweener DOResetLocalPosition(this Transform target, float duration = 0.5f)
        {
            return target.DOLocalMove(Vector3.zero, duration).SetEase(Ease.OutQuad);
        }

        /// <summary>
        /// Tween local scale của Transform về Vector3.one trong duration giây.
        /// </summary>
        public static Tweener DOResetLocalScale(this Transform target, float duration = 0.5f)
        {
            return target.DOScale(Vector3.one, duration).SetEase(Ease.OutBack);
        }

        /// <summary>
        /// Tween alpha của CanvasGroup về 0 (ẩn) trong duration giây, kèm tùy chọn set interactable và blocksRaycasts.
        /// </summary>
        public static Tweener DOFadeOut(this CanvasGroup canvasGroup, float duration = 0.3f, bool disableInteractable = true)
        {
            if (disableInteractable)
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
            return canvasGroup.DOFade(0f, duration).SetEase(Ease.InOutSine);
        }

        /// <summary>
        /// Tween alpha của CanvasGroup về 1 (hiện) trong duration giây, kèm tùy chọn set interactable và blocksRaycasts.
        /// </summary>
        public static Tweener DOFadeIn(this CanvasGroup canvasGroup, float duration = 0.3f, bool enableInteractable = true)
        {
            if (enableInteractable)
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
            return canvasGroup.DOFade(1f, duration).SetEase(Ease.InOutSine);
        }

        /// <summary>
        /// Tween màu fill của Image sang màu targetColor trong duration giây.
        /// </summary>
        public static Tweener DOFillColor(this Image image, Color targetColor, float duration = 0.5f)
        {
            return image.DOColor(targetColor, duration).SetEase(Ease.Linear);
        }

        /// <summary>
        /// Tween alpha của TextMeshProUGUI sang giá trị targetAlpha trong duration giây.
        /// </summary>
        public static Tweener DOFadeAlpha(this TextMeshProUGUI text, float targetAlpha, float duration = 0.5f)
        {
            Color c = text.color;
            return DOTween.To(() => c.a, x =>
            {
                c.a = x;
                text.color = c;
            }, targetAlpha, duration).SetEase(Ease.Linear);
        }

        /// <summary>
        /// Tween anchoredPosition của RectTransform về Vector2.zero trong duration giây.
        /// </summary>
        public static Tweener DOResetAnchoredPosition(this RectTransform rect, float duration = 0.5f)
        {
            return rect.DOAnchorPos(Vector2.zero, duration).SetEase(Ease.OutQuad);
        }

        /// <summary>
        /// Tween xoay local của Transform về Vector3.zero trong duration giây.
        /// </summary>
        public static Tweener DOResetLocalRotation(this Transform target, float duration = 0.5f)
        {
            return target.DOLocalRotate(Vector3.zero, duration).SetEase(Ease.OutQuad);
        }

        /// <summary>
        /// Tween scale local về 0 (ẩn dần) trong duration giây.
        /// </summary>
        public static Tweener DOScaleDown(this Transform target, float duration = 0.3f)
        {
            return target.DOScale(Vector3.zero, duration).SetEase(Ease.InBack);
        }

        /// <summary>
        /// Tween scale local lên 1 (hiện dần) trong duration giây.
        /// </summary>
        public static Tweener DOScaleUp(this Transform target, float duration = 0.3f)
        {
            return target.DOScale(Vector3.one, duration).SetEase(Ease.OutBack);
        }

        /// <summary>
        /// Tween vị trí local sang một vị trí mới trong thời gian duration.
        /// </summary>
        public static Tweener DOMoveLocalTo(this Transform target, Vector3 localPos, float duration = 0.5f)
        {
            return target.DOLocalMove(localPos, duration).SetEase(Ease.OutCubic);
        }

        /// <summary>
        /// Tween xoay local sang góc mới trong duration giây.
        /// </summary>
        public static Tweener DOLocalRotateTo(this Transform target, Vector3 localEulerAngles, float duration = 0.5f)
        {
            return target.DOLocalRotate(localEulerAngles, duration).SetEase(Ease.OutCubic);
        }

        /// <summary>
        /// Tween alpha của Image từ giá trị hiện tại sang targetAlpha trong duration giây.
        /// </summary>
        public static Tweener DOFadeAlpha(this Image image, float targetAlpha, float duration = 0.5f)
        {
            Color c = image.color;
            return DOTween.To(() => c.a, x =>
            {
                c.a = x;
                image.color = c;
            }, targetAlpha, duration).SetEase(Ease.Linear);
        }

        /// <summary>
        /// Tween TextMeshPro alpha với callback gọi khi hoàn thành.
        /// </summary>
        public static Tweener DOFadeAlpha(this TextMeshProUGUI text, float targetAlpha, float duration, TweenCallback onComplete)
        {
            return text.DOFade(targetAlpha, duration).OnComplete(onComplete);
        }

        /// <summary>
        /// Tween hiệu ứng nhấp nháy (flash) màu TextMeshProUGUI giữa 2 màu.
        /// </summary>
        public static Sequence DOFlashColor(this TextMeshProUGUI text, Color flashColor, Color originalColor, int flashCount = 3, float flashDuration = 0.3f)
        {
            Sequence seq = DOTween.Sequence();
            for (int i = 0; i < flashCount; i++)
            {
                seq.Append(text.DOColor(flashColor, flashDuration / 2));
                seq.Append(text.DOColor(originalColor, flashDuration / 2));
            }
            return seq;
        }

        /// <summary>
        /// Tween anchoredPosition của RectTransform sang giá trị mới.
        /// </summary>
        public static Tweener DOAnchorPosTo(this RectTransform rect, Vector2 targetPos, float duration = 0.5f)
        {
            return rect.DOAnchorPos(targetPos, duration).SetEase(Ease.OutCubic);
        }

        /// <summary>
        /// Tween chiều rộng RectTransform sang targetWidth trong duration giây.
        /// </summary>
        public static Tweener DOWidth(this RectTransform rect, float targetWidth, float duration = 0.5f)
        {
            float startWidth = rect.sizeDelta.x;
            return DOTween.To(() => startWidth, x =>
            {
                Vector2 size = rect.sizeDelta;
                size.x = x;
                rect.sizeDelta = size;
            }, targetWidth, duration).SetEase(Ease.OutCubic);
        }

        /// <summary>
        /// Tween chiều cao RectTransform sang targetHeight trong duration giây.
        /// </summary>
        public static Tweener DOHeight(this RectTransform rect, float targetHeight, float duration = 0.5f)
        {
            float startHeight = rect.sizeDelta.y;
            return DOTween.To(() => startHeight, x =>
            {
                Vector2 size = rect.sizeDelta;
                size.y = x;
                rect.sizeDelta = size;
            }, targetHeight, duration).SetEase(Ease.OutCubic);
        }

        /// <summary>
        /// Tween màu Text của Text UI (UnityEngine.UI.Text).
        /// </summary>
        public static Tweener DOTextColor(this Text text, Color targetColor, float duration = 0.5f)
        {
            return text.DOColor(targetColor, duration).SetEase(Ease.Linear);
        }

        /// <summary>
        /// Tween TextMeshProUGUI text thay đổi từ giá trị số từ startValue đến endValue trong duration giây.
        /// </summary>
        public static Tweener DOTextCount(this TextMeshProUGUI text, int startValue, int endValue, float duration = 1f, string format = "{0}")
        {
            return DOVirtual.Int(startValue, endValue, duration, x =>
            {
                text.text = string.Format(format, x);
            }).SetEase(Ease.Linear);
        }
    }
}