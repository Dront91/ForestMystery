using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MysteryForest
{
    public class DestructibleBoss : Destructible
    {
        public Action OnBossDie;

        [Inject] private UICanvasManager _uICanvasManager;

        protected override void Start()
        {
            base.Start();
            this.DamageTaken += OnDamageTaken;
        }

        private void OnDestroy()
        {
            this.DamageTaken -= OnDamageTaken;
        }

        protected override void Die()
        {
            OnBossDie?.Invoke();
            
        }

        private void OnDamageTaken()
        {
            if (_uICanvasManager.TryGetComponent<UIUpdateSnakeHealth>(out var uIUpdateSnakeHealth))
                uIUpdateSnakeHealth.UpdateHealthSnake(MaxHitPoints, _currentHitPoints);
        }
       

        public void BossDieAnimationEnd()
        {
            DieEventAwake();
            if (_uICanvasManager.TryGetComponent<UICanvasManager>(out var canvasManager))
                canvasManager.SwitchCanvas(CanvasType.FinalPrologue);
        }
    }
}