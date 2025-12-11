using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace YNL.Utilities.Extensions
{
    public static class MRectTransform
    {
        public static void WorldToScreenSpace(this RectTransform current, Vector3 target, Camera camMain, RectTransform area, float size)
        {
            Vector3 screenPoint = camMain.WorldToScreenPoint(target);
            screenPoint.z = 0;

            Vector2 screenPos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(area, screenPoint, camMain, out screenPos))
            {
                current.anchoredPosition = screenPos * size;
            }

            current.anchoredPosition = screenPoint * size;
        }

        public static void ReSize(this RawImage target)
        {
            if (target.texture != null)
            {
                Vector2 textureSize = new Vector2(target.texture.width, target.texture.height);
                target.rectTransform.sizeDelta = textureSize;
            }
        }

        public static void ReSize(this Image target, float scale = 1f)
        {
            if (target.sprite != null)
            {
                Vector2 spriteSize = target.sprite.rect.size * scale;
                target.rectTransform.sizeDelta = spriteSize;
            }
        }

        public static void ReSizeByUnit(this Image target)
        {
            if (target.sprite != null)
            {
                float ppu = target.sprite.pixelsPerUnit;
                Vector2 size = target.sprite.rect.size / ppu;
                target.rectTransform.sizeDelta = size * 100f; // nhân 100 để tương ứng với Canvas (nếu dùng Screen Space - Overlay)
            }
        }

        public static Vector2 GetSpriteSize(this Sprite sprite)
        {
            return sprite != null ? sprite.rect.size : Vector2.zero;
        }

        public static void ReSize(this Image image)
        {
            if (image.sprite != null)
            {
                image.rectTransform.sizeDelta = image.sprite.GetSpriteSize();
            }
        }

        public static void ReSize(this SpriteRenderer renderer)
        {
            if (renderer.sprite != null)
            {
                renderer.transform.localScale = renderer.sprite.GetSpriteSize() / 100f;
            }
        }
    }
}