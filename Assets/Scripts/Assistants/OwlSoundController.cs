using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MysteryForest
{
    public class OwlSoundController : BaseEnemySoundController
    {
        private AIOwl _aIOwl;
        protected override void Start()
        {
            _soundHook = GetComponent<SoundHook>();
            _aIOwl = GetComponentInParent<AIOwl>();
            _aIOwl.PlayStepSound += ProgressStepCycle;
        }
        protected override void OnDestroy()
        {
            _aIOwl.PlayStepSound -= ProgressStepCycle;
        }
        protected override void PlayStepSound()
        {
            _audioClip = Sound.OwlFly;
            PlaySound();
        }
    }
}
