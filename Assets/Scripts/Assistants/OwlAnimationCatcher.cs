using UnityEngine;
using System;
namespace MysteryForest
{
    public class OwlAnimationCatcher : MonoBehaviour
    {
        public Action OnTakeOffEnd;
        public Action OnLandingEnd;
        public void AnimationPrepareTakeOffEnd()
        {
            OnTakeOffEnd?.Invoke();
        }
        public void AnimationLandingEnd()
        {
            OnLandingEnd?.Invoke();
        }

    }
}
