using UnityEngine;
using System;
using Zenject;

namespace MysteryForest
{
    public class AIShooter : AIControllerBase
    {       
        [SerializeField] protected float _attackTimeRate;
        [SerializeField] private int _projectileDamage;
        public int ProjectileDamage => _projectileDamage;
        [SerializeField] private float _projectilePushForce;
        public float ProjectilePushForce => _projectilePushForce;
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] protected Transform _projectileSpawnPoint;
        [Inject] private SoundPlayer _soundPlayer;

        protected Animator _animator;
        protected Collider2D _collider;
        protected ShootAnimationCatcher _animationEventCatcher;
        public Action OnConeThrow;

        protected float _timeToShootAttack;

        protected override void Awake()
        {
            base.Awake();

            _animator = GetComponentInChildren<Animator>();
            _animationEventCatcher = GetComponentInChildren<ShootAnimationCatcher>();
            _collider = GetComponent<Collider2D>();
        }

        protected virtual void Start()
        {
            _animationEventCatcher.OnAnimationThrow += SpawnProjectile;
        }

        protected virtual void Update()
        {
            if (ActivItems.Invisibility || Invisibility)
            {
                return;
            }

            FindAttackTarget();

            if (_attackTarget == null)
                return;

            Flip();
            CheckAttackReloaded();            
        }

        protected virtual void OnDestroy()
        {
            _animationEventCatcher.OnAnimationThrow -= SpawnProjectile;
        }

        private void SpawnProjectile()
        {
            Vector3 position;
            if(_attackTarget == null)  
                return; 
            if (_projectileSpawnPoint == null)
                position = transform.position;
            else
                position = _projectileSpawnPoint.position;

            var direction = _attackTarget.transform.position - position;
            float spriteAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            _ = gameObject.Instantiate(_projectilePrefab, position, Quaternion.identity,
                                       _collider, _attackTarget.transform.position, _projectileDamage, 
                                       _projectilePushForce, spriteAngle, _soundPlayer) as GameObject;
        }

        protected override void Flip()
        {           
            if (transform.position.x - _attackTarget.transform.position.x < 0)
            {
                if(_spriteRenderer.flipX != true)
                {
                    FlipProjectileSpawnPoint();
                }
                _spriteRenderer.flipX = true;
            }
            else
            {
                if (_spriteRenderer.flipX != false)
                {
                    FlipProjectileSpawnPoint();
                }
                _spriteRenderer.flipX = false;
            }
        }
        private void FlipProjectileSpawnPoint()
        {
            if (_projectileSpawnPoint != null)
            {
                var startPositionX = _projectileSpawnPoint.localPosition.x;
                _projectileSpawnPoint.localPosition = new Vector3(-startPositionX, _projectileSpawnPoint.localPosition.y, _projectileSpawnPoint.localPosition.z);
            }
        }

        protected void CheckAttackReloaded()
        { 
            if (transform.position.x - _attackTarget.transform.position.x > _npcSightDistance ||
                _timeToShootAttack > Time.time) 
                return;

            Attack();
            _timeToShootAttack = Time.time + _attackTimeRate;
        }

        protected virtual void Attack()
        {
            Shoot();
        }

        protected void Shoot()
        {
            _animator.SetTrigger("throw");
            OnConeThrow?.Invoke();
        }
    }
}
