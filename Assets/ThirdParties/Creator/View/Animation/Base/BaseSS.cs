// This code is part of the SS-Scene library, released by Anh Pham (anhpt.csit@gmail.com).

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Creator
{
    public class BaseSS : MonoBehaviour
    {
        protected bool m_IsPlaying;

        protected float m_AnimationDuration;

        protected float GetFadeDefault() => 0.90f;

        protected float GetDataDefault() => 1;

        protected float GetDataNullDefault() => 0;

        protected virtual void OnEndAnimation()
        {
        }

        protected virtual Tween Effect()
        {
            return null;
        }

        public virtual void Play()
        {
            m_IsPlaying = true;
            
            Sequence mySequence = HelperCreator.DOTweenSequence(gameObject);

            mySequence.Append(Effect());
            
            mySequence.OnComplete(() => { Stop(true); });
        }

        protected virtual void Stop(bool callback = false)
        {
            m_IsPlaying = false;

            if (callback)
            {
                OnEndAnimation();
            }
        }
    }
}