using Creator;
using DG.Tweening;
using UnityEngine;
using UnityUtilities;

public interface ILoading
{
    void ShowLoading();
    void HideLoading();
}

public class PopupLoadingController : Controller, ILoading
{
    public const string SCENE_NAME = "PopupLoading";

    public override string SceneName()
    {
        return SCENE_NAME;
    }

    [SerializeField] Animation m_Anim;

    public override void CreateShield() { }

    private bool m_Init;

    void Start()
    {
        GetCanvasScaler().EditCanvasScaler();
    }

    public void ShowLoading()
    {
        DOTween.KillAll();

        gameObject.SetActive(true);
        if (!m_Init)
        {
            m_Init = true;
        }
        else
        {
        }
    }

    public void HideLoading()
    {
        UnityTimer.Timer.Register(0.35f, () =>
        {
            gameObject.SetActive(false);
        }, autoDestroyOwner: this);
    }
}