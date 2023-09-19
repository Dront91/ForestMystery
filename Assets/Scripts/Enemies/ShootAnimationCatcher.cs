using System;
using UnityEngine;

namespace MysteryForest
{
    public class ShootAnimationCatcher : MonoBehaviour
    {
        public Action OnAnimationThrow;
        public void AnimationThrow()
        {
            OnAnimationThrow?.Invoke();
        }
    }
}
