using UnityEngine;
using UnityEngine.Events;
using System;

namespace MysteryForest
{
    [RequireComponent(typeof(Collider2D))]
    public class Destructible : MonoBehaviour, IPushable
    {
        public event UnityAction<Destructible> OnDeath;
        public Action DamageTaken;
        [SerializeField] protected bool _indestructible;
        [SerializeField] private bool _ispushable;
        [SerializeField] private float _pushResist;
        [SerializeField] protected int _maxHitPoints;
        [SerializeField] private int _spawnSoul;
        public bool _protection;
        public int MaxHitPoints => _maxHitPoints;

        protected float _currentHitPoints;
        public float CurrentHitPoints => _currentHitPoints;
        private PlayerController _playerController;

        public bool IsPushing { 
            get 
            { 
                return _isPushing; 
            } 
            set 
            { 
                _isPushing = value;
            } 
        }
        protected bool _isPushing;

        protected virtual void Start()
        {
            _currentHitPoints = _maxHitPoints;
            _playerController = GetComponent<PlayerController>();
        }

        public virtual void TakeDamage(int damage, float attackPushForce, Vector2 attackPoint)
        {           
            if (_indestructible) return;
            if(_currentHitPoints <= 0) return;

            if(_protection) 
            {
                damage -= (int)(damage * 0.05f);
            }
            _currentHitPoints -= damage;
            DamageTaken?.Invoke();
            if (_currentHitPoints <= 0)
            {
                _currentHitPoints = 0;

                Die();
            }
            if (_ispushable)
            {
                IPushable p = this;
                var pushForce = attackPushForce - _pushResist;
                if(pushForce < 0)
                {
                    pushForce = 0;
                }
                var direction = (Vector2)transform.position - attackPoint;
                p.StartPushing(this, direction, pushForce, 0.5f);
            }
        }

        public void BonusHitPoint(float percentHitPoint) 
        {
            float i = _currentHitPoints * percentHitPoint;

            if (_currentHitPoints + i <= MaxHitPoints)
                _currentHitPoints += i;
            else
                _currentHitPoints = MaxHitPoints;
        }    
        
        protected virtual void Die()
        {
            DieEventAwake();
        }

        protected void DieEventAwake()
        {
            OnDeath?.Invoke(this);
            if (_playerController == null)
            {
                Destroy(gameObject);

                if (TryGetComponent<SpawnSoul>(out var spawnSoul))
                    spawnSoul.Spawn(_spawnSoul);
            }
        }

        public void ChangePushable(bool state)
        {
            _ispushable = state;
        }
    }
}