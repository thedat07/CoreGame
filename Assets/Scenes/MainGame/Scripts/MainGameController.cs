using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTimer;

public class MainGameController : MonoBehaviour
{
        public const string MAIN_GAME = "MainGameCore";

        void Awake()
        {
                Creator.Director director = new Creator.Director();
        }

        void Start()
        {
                Creator.Director.LoadingSceneName = PopupLoadingController.SCENE_NAME;
                Creator.Director.RunScene(DGameController.SCENE_NAME);
        }
}
