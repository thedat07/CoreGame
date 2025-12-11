using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityUtilities;


public class CanvasChangle : MonoBehaviour
{
    public int sortingOrder;

    void Awake()
    {
        SetupCanvas(sortingOrder);
    }

    public void SetupCanvas(int sortingOrder)
    {
        if (gameObject.TryGetComponent<Canvas>(out Canvas canvas))
        {
            canvas.sortingOrder = sortingOrder;
            canvas.worldCamera = Creator.Director.Object.UICamera;

            if (canvas.TryGetComponent<CanvasScaler>(out CanvasScaler canvasScaler))
            {
                canvasScaler.EditCanvasScaler();
            }
        }
    }
}
