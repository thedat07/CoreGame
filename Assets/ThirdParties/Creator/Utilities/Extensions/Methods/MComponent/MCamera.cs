
using UnityEngine;
// using UnityEngine.Rendering.Universal;

namespace YNL.Utilities.Extensions
{
    public static class MCamera
    {

        /// <summary>
        /// Thiết lập camera chính ở chế độ Base và thêm camera UI dạng Overlay vào cameraStack.
        /// </summary>
        /// <param name="mainCamera">Camera chính (Base)</param>
        /// <param name="uiCamera">Camera UI (Overlay)</param>
        public static void SetupBaseAndOverlayCameras(Camera mainCamera, Camera uiCamera)
        {
            // if (mainCamera == null || uiCamera == null)
            // {
            //     Debug.LogWarning("CameraUtilities: mainCamera hoặc uiCamera bị null.");
            //     return;
            // }

            // var mainData = mainCamera.GetUniversalAdditionalCameraData();
            // var uiData = uiCamera.GetUniversalAdditionalCameraData();

            // // Thiết lập kiểu cho từng camera
            // mainData.renderType = CameraRenderType.Base;
            // uiData.renderType = CameraRenderType.Overlay;

            // // Tránh thêm trùng camera nếu đã có trong stack
            // if (!mainData.cameraStack.Contains(uiCamera))
            // {
            //     mainData.cameraStack.Add(uiCamera);
            // }
        }
    }
}