using UnityEngine;
namespace MysteryForest
{
    [RequireComponent(typeof(SoundHook))]
    public class BaseEnemySoundController : MonoBehaviour
    {
        [SerializeField] protected float _stepInterval;
        protected SoundHook _soundHook;
        protected Sound _audioClip;
        protected Fighter _fighter;
        protected float _nextStep;
        protected virtual void Start()
        {
            _soundHook = GetComponent<SoundHook>();
            _fighter = GetComponentInParent<Fighter>();
            if(_fighter != null) 
            {
                _fighter.PlayStepSound += ProgressStepCycle;
            }
        }
        protected virtual void OnDestroy()
        {
            if (_fighter != null)
            {
                _fighter.PlayStepSound -= ProgressStepCycle;
            }
        }
        protected void ProgressStepCycle(float stepCycle)
        {
            if (stepCycle < _nextStep)
                return;
            _nextStep = stepCycle + _stepInterval;
            PlayStepSound();
        }
        protected virtual void PlayStepSound()
        {
            //Override in children classes.
        }
        protected void PlaySound()
        {
            _soundHook.m_Sound = _audioClip;
            _soundHook.Play();
        }
    }
}
