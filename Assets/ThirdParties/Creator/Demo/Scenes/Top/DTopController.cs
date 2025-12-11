using UnityEngine;
using Creator;

public class DTopController : Controller
{
    public const string SCENE_NAME = "DTop";

    public override string SceneName()
    {
        return SCENE_NAME;
    }

    public void OnButtonTap1()
    {
        Creator.Director.PushScene(DPopupController.SCENE_NAME, new DPopupData("Popup1", true), () =>
         {
             Console.Log("Life cycle", "On Show Popup1");
         }, () =>
         {
             Console.Log("Life cycle", "On Hide Popup1");
         }, false);
    }

    public void OnButtonTap2()
    {
        Creator.Director.PushScene(DPopupController.SCENE_NAME, new DPopupData("Popup1", false), () =>
        {
            Console.Log("Life cycle", "On Show Popup1");
        }, () =>
        {
            Console.Log("Life cycle", "On Hide Popup1");
        }, true);

        Creator.Director.PushScene(DPopupController.SCENE_NAME, new DPopupData("Popup2", true), () =>
        {
            Console.Log("Life cycle", "On Show Popup2");
        }, () =>
        {
            Console.Log("Life cycle", "On Hide Popup2");
        }, false);
    }

    public void OnSelectTap()
    {
        Creator.Director.PushScene(DSelectController.DSELECT_SCENE_NAME);
    }

    public override void OnActive(object data)
    {
        Console.Log("Life cycle", SceneName() + " OnActive");
    }

    public override void OnShown()
    {
        Console.Log("Life cycle", SceneName() + " OnShown");
    }

    public override void OnHidden()
    {
        Console.Log("Life cycle", SceneName() + " OnHidden");
    }

    public override void OnReFocus()
    {
        Console.Log("Life cycle", SceneName() + " OnReFocus");
    }
}
