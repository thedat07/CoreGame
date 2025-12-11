using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UniRx;

public static class HelperCreator
{
    public static void StopEverythingInScene()
    {
        // Stop all coroutines on all active MonoBehaviours
        MonoBehaviour[] allBehaviours = Object.FindObjectsOfType<MonoBehaviour>();
        foreach (var behaviour in allBehaviours)
        {
            behaviour.StopAllCoroutines();
        }

        // Kill all active DOTween tweens (also complete them if needed)
        DOTween.KillAll(); // Pass 'true' if you want to complete tweens before killing

        // Optional: reset time scale if it was changed
        Time.timeScale = 1f;

        // Optional: clear DOTween memory (pools, etc.)
        DOTween.Clear();
    }


    public static DG.Tweening.Sequence Register(float delay, UnityAction callBack, GameObject target = null, LinkBehaviour behaviour = LinkBehaviour.KillOnDestroy)
    {
        DG.Tweening.Sequence sequence = DOTween.Sequence();

        sequence.SetLink(target, behaviour);

        sequence.Append(DOVirtual.DelayedCall(delay, () => { callBack?.Invoke(); }));

        return sequence;
    }

    public static DG.Tweening.Sequence DOTweenSequence(GameObject target, LinkBehaviour behaviour = LinkBehaviour.KillOnDestroy)
    {
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.SetLink(target, behaviour);
        return sequence;
    }
}
