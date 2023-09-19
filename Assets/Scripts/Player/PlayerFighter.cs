using System;
using UnityEngine;
using Zenject;

namespace MysteryForest
{
    public class PlayerFighter : Fighter, IPushable
    {
        public Action RestoreHealth;
        [SerializeField] private float _playerRangeAttackDistance;
        [SerializeField] private float _arrowPushForce;
        public float ArrowPushForce => _arrowPushForce;
        [SerializeField] private float _slingshotPushForce;
        public float SlingshotPushForce => _slingshotPushForce;
        public float PlayerRangeAttackDistance => _playerRangeAttackDistance;

        [Inject] private UICanvasManager _uICanvasManager;

        private GodMode _godMode;

        protected override void Start()
        {
            base.Start();

            CheckUpdateHealth();
        }

        private void CheckUpdateHealth()
        {
            if (_uICanvasManager.GetComponent<UIUpdateHealth>() != null)
            {
                _uICanvasManager.GetComponent<UIUpdateHealth>().UpdateHealth(MaxHitPoints, CurrentHitPoints);
            }


            if (_uICanvasManager.TryGetComponent(out _godMode))
                _godMode.OnGodModeEnable += PlayerIndestructibleEnable;                
        }

        private void OnDestroy()
        {
            if(_godMode != null)
                _godMode.OnGodModeEnable -= PlayerIndestructibleEnable;
        }

        private void PlayerIndestructibleEnable(bool state)
        {

            _indestructible = state;
        }

        public void RestoreHitPoints(int value)
        {
            _currentHitPoints += value;
            if (_currentHitPoints > _maxHitPoints)
            {
                _currentHitPoints = _maxHitPoints;
            }
            RestoreHealth?.Invoke();

            CheckUpdateHealth();

        }

        public void IncreaseMaxHitPoints(int value)
        {
            _maxHitPoints += value;
        }
        public override void TakeDamage(int damage, float pushForce, Vector2 attackPoint)
        {
            base.TakeDamage(damage, pushForce, attackPoint);

            CheckUpdateHealth();
        }
    }
}
