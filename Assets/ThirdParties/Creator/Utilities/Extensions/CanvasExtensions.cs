using UnityEngine;
using UnityEngine.UI;

namespace UnityUtilities
{
    public static class CanvasExtensions
    {
        public static void FillAmount(this Slider slider, float value, float defaultShow = 0.1f)
        {
            if (value < defaultShow)
            {
                slider.fillRect.gameObject.SetActive(false);
            }
            else
            {
                slider.fillRect.gameObject.SetActive(true);
                slider.value = Mathf.Clamp(value, defaultShow, 1);
            }
        }

        /// <summary>
        /// Tự động điều chỉnh thuộc tính matchWidthOrHeight của CanvasScaler dựa trên tỉ lệ màn hình hiện tại so với tỉ lệ chuẩn.
        /// Giúp UI scale phù hợp trên nhiều loại thiết bị (Phone, Tablet...).
        /// </summary>
        /// <param name="canvasScaler">CanvasScaler cần chỉnh</param>
        public static void EditCanvasScaler(this CanvasScaler canvasScaler)
        {
            if (NeedsUpdate())
            {
                UnityEngine.Canvas.ForceUpdateCanvases();
            }

            bool NeedsUpdate()
            {
                float currentRatio = (float)Screen.width / Screen.height;
                float defaultRatio = StaticData.ScreenGame.x / StaticData.ScreenGame.y; // 1080/1920 = 0.5625

                float matchValue;

                if (currentRatio < defaultRatio)
                {
                    // Màn hình dài/hẹp hơn chuẩn → cần match chiều RỘNG
                    matchValue = 0f;
                }
                else
                {
                    // Màn hình rộng hơn chuẩn → cần match chiều CAO
                    matchValue = 1f;
                }

                if (!Mathf.Approximately(canvasScaler.matchWidthOrHeight, matchValue))
                {
                    canvasScaler.matchWidthOrHeight = matchValue;
                    return true;
                }

                return false;
            }
        }


        /// <summary>
        /// Đặt layer cho GameObject và tất cả các con của nó theo kiểu đệ quy.
        /// </summary>
        /// <param name="go">GameObject gốc</param>
        /// <param name="layer">Layer muốn đặt</param>
        public static void SetGameLayerRecursive(this GameObject go, int layer)
        {
            if (go == null) return;

            go.layer = layer;

            foreach (Transform child in go.transform)
            {
                // Gọi đệ quy cho từng child
                child.gameObject.SetGameLayerRecursive(layer);
            }
        }

        /// <summary>
        /// Reset lại CanvasScaler về mặc định (matchWidthOrHeight = 1f).
        /// </summary>
        public static void ResetCanvasScaler(this CanvasScaler canvasScaler)
        {
            if (canvasScaler == null) return;

            canvasScaler.matchWidthOrHeight = 1f;
            UnityEngine.Canvas.ForceUpdateCanvases();
        }

        /// <summary>
        /// Thiết lập CanvasScaler để ưu tiên scale theo chiều rộng.
        /// </summary>
        public static void SetMatchWidth(this CanvasScaler canvasScaler)
        {
            if (canvasScaler == null) return;

            canvasScaler.matchWidthOrHeight = 0f; // 0 ưu tiên chiều rộng
            UnityEngine.Canvas.ForceUpdateCanvases();
        }

        /// <summary>
        /// Thiết lập CanvasScaler để ưu tiên scale theo chiều cao.
        /// </summary>
        public static void SetMatchHeight(this CanvasScaler canvasScaler)
        {
            if (canvasScaler == null) return;

            canvasScaler.matchWidthOrHeight = 1f; // 1 ưu tiên chiều cao
            UnityEngine.Canvas.ForceUpdateCanvases();
        }

        public static void EnableInteractable(this CanvasGroup canvasGroup, bool enableInteractable)
        {
            canvasGroup.blocksRaycasts = enableInteractable;
        }
    }
}