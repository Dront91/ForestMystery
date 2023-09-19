using UnityEngine;
using System;
namespace MysteryForest
{
    public class PlayerAnimationCatcher : MonoBehaviour
    {
        public Action<bool> OnAnimationAttackEnd;
        public Action<string> OnAnimationThrow;
        public Action OnAnimationDeadEnd;
        public void AnimationAttackEnded()
        {
            OnAnimationAttackEnd?.Invoke(true);
        }
        public void AnimationThrow(string projectileType)
        {
            OnAnimationThrow?.Invoke(projectileType);
        }
        public void AnimationDeadEnd()
        {
            OnAnimationDeadEnd?.Invoke();
        }
    }
}
