using UnityEngine;
using System;
namespace MysteryForest
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Fighter : Destructible
    {
        public Action<float> PlayStepSound;
        public float _moveSpeed;
        public float _maxMoveSpeed;
        [SerializeField] private Transform _attackPoint;
        private float _stepCycle;
        [HideInInspector]public float _maxSpeed;
        [SerializeField] protected int _damage;
        public int Damage => _damage;
        [SerializeField] protected float _pushForce;
        public float PushForce => _pushForce;
        [SerializeField] protected float _attackRate;
        public float AttackRate => _attackRate;
        [SerializeField] private int _creteChance = 50;
        public int CreteChance => _creteChance;
        [SerializeField] private float _criticalDamageCoef = 1.5f;
        public float CriticalDamageCoef => _criticalDamageCoef;
        


        public Vector2 MoveVector { get; set; }
        private Collider2D _collider;
        public Collider2D Collider => _collider;
        private Rigidbody2D _rigidbody;
        public Rigidbody2D Rigidbody => _rigidbody;

        protected override void Start()
        {
            base.Start();

            _maxSpeed = _moveSpeed;
            _collider = GetComponent<Collider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            UpdatePosition();
            CheckMovementStatus();
        }

        private void Update()
        {
            RotateAttackPoint();
        }
        private void CheckMovementStatus()
        {
            if( MoveVector != Vector2.zero)
            {
                _stepCycle += _moveSpeed * Time.fixedDeltaTime;
            }
            PlayStepSound?.Invoke(_stepCycle);
        }

        private void UpdatePosition()
        {
            transform.Translate(MoveVector * _moveSpeed / 100);
        }

        private void RotateAttackPoint()
        {
            if(_attackPoint != null && MoveVector != Vector2.zero)
                _attackPoint.transform.right = MoveVector;
        }

        public void DefoldSpeed()
        {
            _moveSpeed = _maxMoveSpeed;
        }
        public void UpdateCreteChance(float value)
        {
            _creteChance += (int)(_creteChance * value); 
        }
        public void UpdateCreteDamageCoef(float value)
        {
            _criticalDamageCoef += _criticalDamageCoef * value;
        }
        public void BonusAttack(float percentAttack)
        {
            _damage += (int)(_damage * percentAttack);
        }
    }
}