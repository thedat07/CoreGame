using Creator;

public class TemplateDataController : Controller
{
    public const string TEMPLATE_SCENE_NAME = "TemplateData";

    public override string SceneName()
    {
        return TEMPLATE_SCENE_NAME;
    }

    private TemplateData m_Data;

    public override void OnActive(object data)
    {
        if (m_Data != null)
        {
            m_Data = data as TemplateData;
        }
        else
        {
            m_Data = new TemplateData();
        }
    }
}