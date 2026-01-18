using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTimer;

public class MainGameController : MonoBehaviour
{
        IEnumerator Start()
        {
                yield return WaitForFrames(10);

                Creator.Director.Init();
        }

        IEnumerator WaitForFrames(int frameCount)
        {
                for (int i = 0; i < frameCount; i++)
                        yield return null;
        }
}
