using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraResizer : MonoBehaviour
{
    [Header("Target Aspect")]
    public float targetWidth = 1080f;
    public float targetHeight = 1920f;

    [Header("Orthographic Settings")]
    public float pixelsPerUnit = 100f;
    public float minOrthographicSize = 3f;
    public float maxOrthographicSize = 10f;

    [Header("Perspective Settings")]
    [Tooltip("Min–max clamp cho FOV")]
    public float minFOV = 40f;
    public float maxFOV = 80f;

    private Camera cam;
    private float defaultFOV;

    void Awake()
    {
        cam = GetComponent<Camera>();
        // Lấy FOV hiện tại làm mặc định
        defaultFOV = cam.fieldOfView;
    }

    void Start()
    {
        Resize();
    }

    void Resize()
    {
        float targetAspect = targetWidth / targetHeight;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scale = windowAspect / targetAspect;

        if (cam.orthographic)
        {
            // --- Orthographic ---
            if (scale < 1.0f)
            {
                cam.orthographicSize = (targetHeight / 2f) / pixelsPerUnit;
            }
            else
            {
                cam.orthographicSize = (targetHeight / 2f) / pixelsPerUnit * scale;
            }

            // Clamp min-max
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minOrthographicSize, maxOrthographicSize);
        }
        else
        {
            // --- Perspective ---
            float fov = defaultFOV;

            // nếu màn hình rộng hơn tỉ lệ target thì mở rộng FOV
            if (scale > 1.0f)
                fov *= scale;

            // Clamp min-max
            cam.fieldOfView = Mathf.Clamp(fov, minFOV, maxFOV);
        }
    }
}
