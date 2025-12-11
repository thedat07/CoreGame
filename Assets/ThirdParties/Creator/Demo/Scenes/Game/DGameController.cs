using System.Collections;
using UnityEngine;
using Creator;

public class DGameController : Controller
{
    public const string SCENE_NAME = "DGame";

    public override string SceneName()
    {
        return SCENE_NAME;
    }

    void Start()
    {
        Console.Log("Life cycle", SceneName() + " Start");

        StartCoroutine(OnStart());
    }

    void OnEnable()
    {
        Console.Log("Life cycle", SceneName() + " OnEnable");
    }

    void Awake()
    {
        Console.Log("Life cycle", SceneName() + " Awake");
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

    void OnDisable()
    {
        Console.Log("Life cycle", SceneName() + " OnDisable");
    }

    void OnDestroy()
    {
        Console.Log("Life cycle", SceneName() + " OnDestroy");
    }

    public void OnButtonTap()
    {
        Creator.Director.PushScene(DTopController.SCENE_NAME);
    }

    IEnumerator OnStart()
    {
        yield return new WaitForSeconds(1f);

        Creator.Director.LoadingAnimation(false);
    }
}