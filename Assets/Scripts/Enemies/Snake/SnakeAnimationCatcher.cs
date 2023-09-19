using System;

namespace MysteryForest
{
    public class SnakeAnimationCatcher : ShootAnimationCatcher
    {
        public Action OnFlipEnd;
        public Action OnJump;
        public void AnimationFlipEnd()
        {
            OnFlipEnd?.Invoke();
        }
         public void AnimationJump()
        {
            OnJump?.Invoke();
        }
    }
}