using UnityEngine;
using DesignPatterns;
using UniRx;
using System.Collections;

public partial class GameManager : SingletonPersistent<GameManager>
{
    [SerializeField] SettingModel m_SettingModel;
    [SerializeField] AdsModel m_AdsModel;


    [Header("ModelView")]
    [SerializeField] AdsModelView m_AdsModelView;
    [SerializeField] ShopModelView m_ShopModelView;

    public ShopModelView GetShopModelView() => m_ShopModelView;
    public AdsModelView GetAdsModelView() => m_AdsModelView;

    public SettingModel GetSettingData() => m_SettingModel;
    public AdsModel GetAdsData() => m_AdsModel;

    IEnumerator Start()
    {
        yield return null; // frame đầu render

        InitLight();

        yield return null;
        yield return null;

        InitModels();

        yield return null;

        InitViews();

        yield return null;

        InitSystem();

        yield return DelayFrames(5);

        yield return null;

        ApplyQuality();

        yield return null;

        Creator.Director.RunScene(DGameController.SCENE_NAME);
    }

    IEnumerator DelayFrames(int frames)
    {
        for (int i = 0; i < frames; i++)
            yield return null;
    }

    void InitLight()
    {

    }

    void InitModels()
    {
        m_SettingModel = new SettingModel();
        m_AdsModel = new AdsModel();
    }

    void InitViews()
    {
        m_AdsModelView?.Initialize();
        m_ShopModelView?.Initialize();
    }

    void InitSystem()
    {

    }

    void ApplyQuality()
    {
        int qualityIndex = DetectQualityLevel();
        QualitySettings.SetQualityLevel(qualityIndex, true);

        // Cài FPS theo từng mức
        switch (qualityIndex)
        {
            case 0: // Low
                Application.targetFrameRate = 30;
                break;
            case 1: // Medium
                Application.targetFrameRate = 60;
                break;
            case 2: // High
                Application.targetFrameRate = 60; // hoặc 120 nếu muốn cho device cao cấp
                break;
        }

        QualitySettings.vSyncCount = 0;
        Console.Log($"👉 Applied Quality: {QualitySettings.names[qualityIndex]} | FPS: {Application.targetFrameRate}");
    }

    int DetectQualityLevel()
    {
#if UNITY_EDITOR
        return 2; // Medium khi chạy Editor
#elif UNITY_ANDROID
        // Android phân loại theo RAM + GPU
        if (SystemInfo.systemMemorySize < 3000 || SystemInfo.graphicsMemorySize < 500)
            return 0; // Low
        else if (SystemInfo.systemMemorySize < 5000)
            return 1; // Medium
        else
            return 2; // High
#elif UNITY_IOS
        // iOS phân loại theo model
        string device = SystemInfo.deviceModel.ToLower();
        if (device.Contains("iphone6") || device.Contains("iphone7") || device.Contains("ipad5"))
            return 0; // Low
        else if (device.Contains("iphone8") || device.Contains("iphone9") || device.Contains("ipad6"))
            return 1; // Medium
        else
            return 2; // High
#else
        return 1; // Default Medium
#endif
    }
}
