using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MysteryForest
{
    public class Arrow : Projectile
    {
        [SerializeField] private float _soundDelay;
        private SoundHook _soundHookArrowSound;
        private float _timer;
        private bool _firstUse = true;
        protected override void Start()
        {
            base.Start();
            _soundHookArrowSound = GetComponentInChildren<SoundHook>();
        }
        protected override void Update()
        {
            base.Update();
            _timer += Time.deltaTime;
            if(_timer > _soundDelay && _firstUse)
            {
                _timer = 0;
                _firstUse = false;
                _soundPlayer.Play(_soundHookArrowSound.m_Sound);
            }
        }
    }
}
