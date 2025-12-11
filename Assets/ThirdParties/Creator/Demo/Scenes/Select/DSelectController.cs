using UnityEngine;
using Creator;

public class DSelectController : Controller
{
  public const string DSELECT_SCENE_NAME = "DSelect";

  public override string SceneName()
  {
    return DSELECT_SCENE_NAME;
  }

  public void OnGameButtonTap()
  {
    Creator.Director.RunScene(DGameController.SCENE_NAME);
  }

  public override void OnActive(object data = null)
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
}