using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MysteryForest
{
    public class SnakeSoundController : BaseEnemySoundController
    {
        private AISnake _aISnake;
        private Destructible _destructible;
        
        protected override void Start()
        {
            base.Start();
            _aISnake = GetComponentInParent<AISnake>();
            _destructible = GetComponentInParent<Destructible>();
            AddListners();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            RemoveListners();
        }
        private void AddListners()
        {
            _aISnake.OnShoot += OnShoot;
            _aISnake.OnRushAttackStart += OnRushStart;
            _aISnake.OnRushAttackDeal += OnMeleeAttack;
            _aISnake.OnJump += OnJump;
            _aISnake.OnLanding += OnLanding;
            _destructible.OnDeath += OnDeath;
            _destructible.DamageTaken += OnDamageTaken;
        }
        private void RemoveListners()
        {
            _aISnake.OnShoot -= OnShoot;
            _aISnake.OnRushAttackStart -= OnRushStart;
            _aISnake.OnRushAttackDeal -= OnMeleeAttack;
            _aISnake.OnJump -= OnJump;
            _aISnake.OnLanding -= OnLanding;
            _destructible.OnDeath -= OnDeath;
            _destructible.DamageTaken -= OnDamageTaken;
        }

        private void OnLanding()
        {
            _audioClip = Sound.SnakeLanding;
            PlaySound();
        }

        private void OnShoot()
        {
            _audioClip = Sound.SnakeShoot;
            PlaySound();
        }

        private void OnRushStart(int obj)
        {
            _audioClip = Sound.SnakeRushPrepare;
            PlaySound();
        }

        private void OnMeleeAttack()
        {
            _audioClip = Sound.SnakeMeleeAttack;
            PlaySound();
        }

        private void OnJump()
        {
            _audioClip = Sound.SnakeJump;
            PlaySound();
        }

        private void OnDeath(Destructible arg0)
        {
            _audioClip = Sound.SnakeDead;
            PlaySound();
        }

        private void OnDamageTaken()
        {
            _audioClip = Sound.SnakeDamageResive;
            PlaySound();
        }
    }
}
