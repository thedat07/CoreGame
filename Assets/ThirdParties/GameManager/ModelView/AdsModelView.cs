using UnityEngine;
using UnityEngine.Events;
using Gley.MobileAds;
using UnityTimer;
using DG.Tweening;

public class AdTimer
{
    private static System.DateTime _adStartTime;

    public static void MarkStart()
    {
        _adStartTime = System.DateTime.UtcNow;
    }

    public static float GetDuration()
    {
        if (_adStartTime == default)
            return 0f;

        return (float)(System.DateTime.UtcNow - _adStartTime).TotalSeconds;
    }
}

public class AdsModelView : MonoBehaviour
{
    public AdsInfo adsInfoData;

    AdsModel m_Model;

    public string placementInter = "";
    public string placementReward = "";

    public float interstitialStartTime;
    public float rewardedStartTime;


    [SerializeField] GameObject m_ShieldAds;

    public void UpdateLastAdTime(bool isRewardedAd) => adsInfoData.UpdateLastAdTime(isRewardedAd);

    public void Initialize()
    {
        m_Model = GameManager.Instance.GetAdsData();
        Gley.MobileAds.API.Initialize(OnInitialized);
        void OnInitialized()
        {
            //Show ads only after this method is called
            //This callback is not mandatory if you do not want to show banners as soon as your app starts.
            // ShawBanner();
        }
    }

    public void ShowMediationDebugger()
    {
      
    }

    public int UpdateAdShowedCount()
    {
        m_Model.adShowedCount++;

        return m_Model.adShowedCount;
    }

    public int UpdateAdInterCount()
    {
        adsInfoData.adInterAds++;
        m_Model.adInterCount++;
        return m_Model.adInterCount;
    }

    [ContextMenu("Remove Ads")]
    public void OnRemoveAds()
    {
        Gley.MobileAds.API.RemoveAds(true);
    }

    public void ShawBanner()
    {
        Gley.MobileAds.API.ShowBanner(BannerPosition.Bottom, BannerType.Banner);
    }

    public void HideBanner()
    {
        Gley.MobileAds.API.HideBanner();
    }

    public void ShowInterstitial(string placement)
    {
        if (adsInfoData.CanShowInterstitialAd())
        {
            SetActiveShield(true);

            placementInter = placement;

            Timer.Register(StaticData.DelayTimeDefault, () =>
            {
                SetActiveShield(false);
                interstitialStartTime = Time.time;
                Gley.MobileAds.API.ShowInterstitial(() => { UpdateLastAdTime(false); });
            }, autoDestroyOwner: GameManager.Instance);
        }
    }

    public void ShowInterstitial(string placement, UnityAction interstitialClosed)
    {
        if (adsInfoData.CanShowInterstitialAd())
        {
            SetActiveShield(true);

            placementInter = placement;

            Timer.Register(StaticData.DelayTimeDefault, Show, autoDestroyOwner: GameManager.Instance);
        }

        void Show()
        {
            SetActiveShield(false);
            Gley.MobileAds.API.ShowInterstitial(() =>
            {
                UpdateLastAdTime(false);
                interstitialClosed?.Invoke();
            });
        }
    }

    public void ShowRewardedVideo(string placement, UnityAction onSuccess = null, UnityAction onFail = null, UnityAction onCompleted = null)
    {
        SetActiveShield(true);

        placementReward = placement;

        Timer.Register(StaticData.DelayTimeDefault, Show, autoDestroyOwner: GameManager.Instance);

        void Show()
        {
            SetActiveShield(false);
            rewardedStartTime = Time.time;
            Gley.MobileAds.API.ShowRewardedVideo(CompleteMethod);
        }

        void CompleteMethod(bool completed)
        {
            Callback(completed);
        }

        void Callback(bool completed)
        {
            if (completed)
            {
                onSuccess?.Invoke();
                UpdateLastAdTime(true);
            }
            else
            {
                onFail?.Invoke();
            }

            onCompleted?.Invoke();
        }
    }

    private Tween m_AutoOffTween;

    private Tween m_DeactivateTween;

    void SetActiveShield(bool active)
    {
        if (active)
        {
            m_ShieldAds.gameObject.SetActive(true);

            m_AutoOffTween?.Kill();
            m_DeactivateTween?.Kill();


            m_AutoOffTween = DOVirtual.DelayedCall(1f, () =>
            {
                if (m_ShieldAds.gameObject.activeSelf)
                {
                    SetActiveShield(false);
                }
            }).SetLink(gameObject);
        }
        else
        {
            m_AutoOffTween?.Kill();
            m_AutoOffTween = null;

            m_DeactivateTween?.Kill();
            m_DeactivateTween = DOVirtual.DelayedCall(StaticData.DelayTimeDefault, () =>
            {
                m_ShieldAds.gameObject.SetActive(false);
            }).SetLink(gameObject);
        }
    }
}
