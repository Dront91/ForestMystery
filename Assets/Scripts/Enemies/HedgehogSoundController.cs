
using System;
using UnityEngine;
namespace MysteryForest
{
    [RequireComponent(typeof(AudioSource))]
    public class HedgehogSoundController : BaseEnemySoundController
    {
        [SerializeField] private Sounds _soundList;
        private Destructible _destructible;
        private AIHedgehog _hedgehogAI;
        private AudioSource _audioSource;
        
        protected override void Start()
        {
            base.Start();
            _destructible = GetComponentInParent<Destructible>();
            _hedgehogAI = GetComponentInParent<AIHedgehog>();
            _audioSource = GetComponent<AudioSource>();
            _destructible.DamageTaken += OnDamageTaken;
            _hedgehogAI.OnRollStart += OnRollStart;
            _hedgehogAI.OnRollEnd += OnRollEnd;
            _hedgehogAI.OnStunStart += OnStunStart;
            _hedgehogAI.OnStunEnd += OnStunEnd;
            _hedgehogAI.OnWallCrash += OnWallCrash;
            _hedgehogAI.OnRushStart += OnRushStart;
            
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            _destructible.DamageTaken -= OnDamageTaken;
            _hedgehogAI.OnRollStart -= OnRollStart;
            _hedgehogAI.OnRollEnd -= OnRollEnd;
            _hedgehogAI.OnStunStart -= OnStunStart;
            _hedgehogAI.OnStunEnd -= OnStunEnd;
            _hedgehogAI.OnWallCrash -= OnWallCrash;
            _hedgehogAI.OnRushStart -= OnRushStart;
        }

        private void OnRushStart()
        {
            _audioClip = Sound.HedgehogRush;
            PlaySound();
        }

        private void OnRollStart()
        {
            _audioSource.clip = _soundList[Sound.HedgehogRoll]._clip;
            _audioSource.loop = true;
            _audioSource.Play();
        }

        private void OnRollEnd()
        {
            _audioSource.Stop();
        }

        private void OnStunStart()
        {
            _audioSource.clip = _soundList[Sound.HedgehogStun]._clip;
            _audioSource.loop = true;
            _audioSource.Play();
        }
        private void OnStunEnd()
        {
            _audioSource.Stop();
        }

        private void OnWallCrash()
        {
            _audioClip = Sound.HedgehogCrash;
            PlaySound();
        }

        private void OnDamageTaken()
        {
            _audioClip = Sound.HedgehogDamageResive;
            PlaySound();
        }
    }
}
