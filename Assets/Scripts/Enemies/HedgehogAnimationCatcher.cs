using System;
using UnityEngine;

namespace MysteryForest
{
    public class HedgehogAnimationCatcher : MonoBehaviour
    {
        public Action OnAnimationJumpFoldEnd;
        public Action OnAnimationUnfoldEnd;
        public void AnimationJumpFoldEnded()
        {
            OnAnimationJumpFoldEnd?.Invoke();
        }

        public void AnimationUnfoldEnded()
        {
            OnAnimationUnfoldEnd?.Invoke();
        }
    }
}
