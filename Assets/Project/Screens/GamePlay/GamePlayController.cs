using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.UI;

public class GamePlayController : MonoBehaviour, IKeyBack
{
    public const string NAME = "GamePlay";

    public void OnKeyBack()
    {
        Core.Close();
    }
}