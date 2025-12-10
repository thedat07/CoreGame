using UnityEngine;
using DesignPatterns;

public partial class GameManager : SingletonPersistent<GameManager>
{
    [SerializeField] SettingModel m_SettingModel;
    [SerializeField] AdsModel m_AdsModel;


    [Header("ModelView")]
    [SerializeField] AdsModelView m_AdsModelView;
    [SerializeField] ShopModelView m_ShopModelView;

    public override void Awake()
    {
        Init();
    }

    public void Init()
    {
        m_AdsModel = new AdsModel();

        m_AdsModelView.Initialize();
        m_ShopModelView.Initialize();
    }

    void Start()
    {
    }

    public ShopModelView GetShopModelView() => m_ShopModelView;
    public AdsModelView GetAdsModelView() => m_AdsModelView;

    public SettingModel GetSettingData() => m_SettingModel;
    public AdsModel GetAdsData() => m_AdsModel;
}
