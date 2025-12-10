using UnityEngine;
using UnityUtilities;
using UniRx;

public class AdsModel
{
    public int adShowedCount
    {
        set => PlayerPrefs.SetInt("AdShowedCount", value);
        get => PlayerPrefs.GetInt("AdShowedCount", 0);
    }

    public int adInterCount
    {
        set => PlayerPrefs.SetInt("AdInterCount", value);
        get => PlayerPrefs.GetInt("AdInterCount", 0);
    }

    private const string KeyRemoveAds = "keyRemoveAds";

    private const bool DefaultRemoveAds = false;

    private readonly ReactiveProperty<bool> _isRemoveAds;
    public IReadOnlyReactiveProperty<bool> IsRemoveShowAds => _isRemoveAds;

    public AdsModel()
    {
        // Khởi tạo từ Save hoặc mặc định
        _isRemoveAds = new ReactiveProperty<bool>(SaveExtensions.GetAds(KeyRemoveAds, DefaultRemoveAds));

        // Tự động lưu khi giá trị thay đổi
        _isRemoveAds.Subscribe(val => SaveExtensions.PutAds(KeyRemoveAds, val));
    }

    /// <summary>
    /// Gán giá trị thủ công từ code
    /// </summary>
    public void SetRemoveAds(bool value)
    {
        _isRemoveAds.Value = value;
    }

    /// <summary>
    /// DELETE: Đặt lại trạng thái quảng cáo
    /// </summary>
    public void Reset()
    {
        SaveExtensions.PutAds(KeyRemoveAds, DefaultRemoveAds);
    }
}

[System.Serializable]
public class AdsInfo
{
    public int adInterAds;

    private float lastAdTime = -999f; // Khởi tạo ban đầu để chắc chắn ads có thể hiển thị từ đầu

    /// <summary>
    /// Kiểm tra xem có thể hiện quảng cáo interstitial không.
    /// </summary>
    public bool CanShowInterstitialAd()
    {
        if (StaticData.SeassonGame <= 1) return false;
        int currentLevel = 10;
        int minLevelForAds = StaticData.LevelStartShowingInter;

        bool hasReachedMinLevel = currentLevel >= minLevelForAds;
        bool isAdCooldownOver = Time.time >= lastAdTime;
        bool canAdBeShown = Gley.MobileAds.API.CanShowAds();

        bool hasPlayedEnoughTime = Time.time >= StaticData.MinGameTimeForInter;
                                
        return hasReachedMinLevel && isAdCooldownOver && canAdBeShown && hasPlayedEnoughTime;
    }

    /// <summary>
    /// Cập nhật thời gian hiển thị quảng cáo gần nhất.
    /// </summary>
    /// <param name="isRewardedAd">True nếu là reward ad, false nếu là interstitial</param>
    public void UpdateLastAdTime(bool isRewardedAd)
    {
        float cooldown = isRewardedAd ? StaticData.InterTimestepRw : StaticData.InterTimestep;
        lastAdTime = Time.time + cooldown;
    }
}