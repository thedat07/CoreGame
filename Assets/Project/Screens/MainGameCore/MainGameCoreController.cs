using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.UI;

public class MainGameCoreController : MonoBehaviour
{
    public const string NAME = "MainGameCore";

    void Awake()
    {
        Core.Init();
    }

    void Start()
    {
        Core.Load<GamePlayController>(sceneName: "GamePlay");
    }
}