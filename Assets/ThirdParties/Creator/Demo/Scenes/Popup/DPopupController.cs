using UnityEngine;
using UnityEngine.UI;
using Creator;

public class DPopupData
{
    public string title;
    public bool activeCloseFull;

    public DPopupData(string title, bool activeCloseFull)
    {
        this.title = title;
        this.activeCloseFull = activeCloseFull;
    }
}

public class DPopupController : Controller
{
    [SerializeField] Text m_Text;

    [SerializeField] GameObject m_CloseFul;

    public const string SCENE_NAME = "DPopup";

    DPopupData m_Data;

    public override string SceneName()
    {
        return SCENE_NAME;
    }

    public override void OnActive(object data = null)
    {
        if (data != null)
        {
            m_Data = data as DPopupData;
            m_Text.text = m_Data.title;
            m_CloseFul.SetActive(m_Data.activeCloseFull);
        }
        Console.Log("Life cycle", data + " OnActive");
    }

    public override void OnShown()
    {
        Console.Log("Life cycle", m_Data + " OnShown");
    }

    public override void OnHidden()
    {
        Console.Log("Life cycle", m_Data + " OnHidden");
    }

    public override void OnReFocus()
    {
        Console.Log("Life cycle", m_Data + " OnReFocus");
    }

    public void CloseTwice()
    {
        Creator.Director.PopToRootScene();
    }
}