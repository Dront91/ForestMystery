using System;
using System.Collections;
using UnityEngine;

namespace MysteryForest
{
    [RequireComponent(typeof(DestructibleBoss))]
    public class AISnake : AIShooter
    {
        private enum AttackType
        {
            Shoot,
            Rush,
            Jump,
            Null
        }
        [SerializeField] private float _rushSpeed;
        [SerializeField] private float _jumpSpeed;
        [SerializeField] private float _minRushDistance;
        [SerializeField] private int _rushDamage;
        [SerializeField] private float _rushPushForce;
        public float RushPushForce => _rushPushForce;
        [SerializeField] private float _specialAttackReloadTime;
        [SerializeField] private float _landingDamageRadius;
        [SerializeField] private int _landingDamage;
        [SerializeField] private float _landingPushForce;
        [SerializeField] private GameObject _snakeLandingShadow;
        [SerializeField] private GameObject _landingEffect;
        [SerializeField] private float _deathTime;

        public Action<int> OnRushAttackStart;
        public Action OnRushAttackEnd;
        public Action OnShoot;
        public Action OnRushAttackDeal;
        public Action OnJump;
        public Action OnLanding;

        private DestructibleBoss _destructible;
        private AttackType _currentAttackType;
        private Vector2 _rushTarget;
        private Vector2 _jumpAttackTarget;
        private Vector2 _jumpHighestPoint;

        private float _timeToSpecialAttack;
        private bool _specialAttackReady = false;
        private bool _landingAnimStart = false;
        private int _sortingOrder;

        private const float _offsetRushTarget = 1f;
        private const float _rushAnimationSpeedRatio = 0.8f;
        private const float _jumpDistance = 11f;
        private const float _pivotOffsetY = 1.1f;
        private const float _landingTimeOffset = 1f;
        private const float _landingAnimStartOffset = 5f;
        
        protected override void Start()
        {
            base.Start();
            _destructible = GetComponent<DestructibleBoss>();
            _currentAttackType = AttackType.Null;

            (_animationEventCatcher as SnakeAnimationCatcher).OnFlipEnd += FlipEnd;
            (_animationEventCatcher as SnakeAnimationCatcher).OnJump += JumpStart;

            _destructible.OnBossDie += OnDeathAnimationStart;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            (_animationEventCatcher as SnakeAnimationCatcher).OnFlipEnd -= FlipEnd;
            (_animationEventCatcher as SnakeAnimationCatcher).OnJump -= JumpStart;

            if (_destructible!=null)
                _destructible.OnBossDie -= OnDeathAnimationStart;
        }

        protected override void Update()
        {
            FindAttackTarget();

            if (_attackTarget == null)
                return;

            Flip();
            CheckAttackReloaded();
            
            _timeToSpecialAttack += Time.deltaTime;
            if (_specialAttackReloadTime < _timeToSpecialAttack)
                _specialAttackReady = true;           
        }

        protected override void Attack()
        {
            if (_currentAttackType != AttackType.Null) 
                return;

            if (_specialAttackReady && _destructible.IsPushing == false)
            {
                int random = UnityEngine.Random.Range(0, 2);
                if (random == 0)
                {
                    JumpAttackPrepared();
                }
                if (random == 1)
                {
                    _rushTarget = _attackTarget.transform.position
                        - (_attackTarget.transform.position - transform.position).normalized * _offsetRushTarget;

                    var hit = Physics2D.Raycast(transform.position, (_rushTarget - (Vector2)transform.position) 
                        / Vector2.Distance(transform.position, _rushTarget), 
                          Vector2.Distance(transform.position, _rushTarget));

                    if (Vector2.Distance(_attackTarget.transform.position, transform.position) > _minRushDistance
                                         && hit == false)
                        RushAttackPrepare();
                    else
                        JumpAttackPrepared();
                }
                return;
            }
            OnShoot?.Invoke();
            Shoot();
        }

        private void RushAttackPrepare()
        {
            OnRushAttackStart?.Invoke(_rushDamage);
            _destructible.ChangePushable(false);
            _animator.SetTrigger("rush");
            _animator.speed = _rushSpeed * _rushAnimationSpeedRatio /
                              (_attackTarget.transform.position - transform.position).magnitude;

            _currentAttackType = AttackType.Rush;
            _specialAttackReady = false;
            _timeToSpecialAttack = 0;

            StartCoroutine(RushAttack());

        }

        private IEnumerator RushAttack()
        {
            OnRushAttackDeal?.Invoke();
            while (Vector2.Distance(transform.position, _rushTarget) > 0.5f)
            {
                transform.Translate(_rushSpeed * Time.deltaTime * (_rushTarget - (Vector2)transform.position).normalized);
                yield return null;
            }

            _currentAttackType = AttackType.Null;
            _destructible.ChangePushable(true);
            OnRushAttackEnd?.Invoke();
        }

        private void JumpAttackPrepared()
        {
            _destructible.ChangePushable(false);
            _animator.SetTrigger("jump");
            _currentAttackType = AttackType.Jump;
            _jumpHighestPoint = new Vector2(transform.position.x, transform.position.y + _jumpDistance);

            _specialAttackReady = false;
            _timeToSpecialAttack = 0;
        }

        private IEnumerator JumpAttack()
        {
            _sortingOrder = _spriteRenderer.sortingOrder;
            _spriteRenderer.sortingOrder = 11; //Пролет змеи над деревьями
            _collider.enabled = false;

            while ((_jumpHighestPoint.y - transform.position.y) > 0.1f)
            {
                transform.Translate(_jumpSpeed * Time.deltaTime * transform.up);
                yield return null;
            }

            FindTargetForLanding();
            transform.position = new Vector2(_attackTarget.transform.position.x,
                                             _attackTarget.transform.position.y + _jumpDistance);
            FlipEnd();
            StartCoroutine(Landing());
        }

        private void FindTargetForLanding()
        {
            _jumpAttackTarget = _attackTarget.transform.position;
        }

        private IEnumerator Landing()
        {
            if (_snakeLandingShadow != null)
                Instantiate(_snakeLandingShadow, _jumpAttackTarget - new Vector2(0, _pivotOffsetY), Quaternion.identity);

            yield return new WaitForSeconds(_landingTimeOffset);

            while ((transform.position.y - _jumpAttackTarget.y) > 0.1f)
            {
                if ((transform.position.y - _jumpAttackTarget.y) < _landingAnimStartOffset && !_landingAnimStart)
                {
                    _animator.SetTrigger("landing");
                    _landingAnimStart = true;
                }

                transform.Translate(_jumpSpeed * Time.deltaTime * -transform.up);
                yield return null;
            }
            OnLanding?.Invoke();
            if (_landingEffect != null)
                Instantiate(_landingEffect, transform.position, Quaternion.identity);

            ApplyDamageOnLanding();

            _currentAttackType = AttackType.Null;
            _spriteRenderer.sortingOrder = _sortingOrder;
            _collider.enabled = true;
            _destructible.ChangePushable(true);
            _landingAnimStart = false;
        }

        private void ApplyDamageOnLanding()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _landingDamageRadius);

            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent(out Destructible target))
                {
                    target.TakeDamage(_landingDamage, _landingPushForce, transform.position );
                }
            }
        }

        protected override void Flip()
        {
            if (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "anim_snake_idle") 
                return;

            if (_animator.speed != 1f)
                _animator.speed = 1f;

            if (transform.position.x - _attackTarget.transform.position.x < 0)
            {
                if (!_spriteRenderer.flipX)
                    _animator.SetTrigger("flip");
            }
            else
            {
                if (_spriteRenderer.flipX)
                    _animator.SetTrigger("flip");
            }
        }
        private void FlipEnd()
        {
            _projectileSpawnPoint.localPosition = new Vector2(-_projectileSpawnPoint.localPosition.x, 
                                                              _projectileSpawnPoint.localPosition.y);
            if(!_spriteRenderer.flipX)            
                 _spriteRenderer.flipX = true;            
            else
                _spriteRenderer.flipX = false;         
        }

        private void JumpStart()
        {
            OnJump?.Invoke();
            StartCoroutine(JumpAttack());
        }
        private void OnDeathAnimationStart()
        {
            _animator.SetTrigger("death");

            _currentAttackType = AttackType.Null;
            AIEnable(false);
            _collider.enabled = false;

            StartCoroutine(DeathAnimation());
        }

        private IEnumerator DeathAnimation()
        {
            yield return new WaitForSeconds(_deathTime);

            _destructible.BossDieAnimationEnd();
        }

#if UNITY_EDITOR
        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();

            if(_rushTarget != null)
                Gizmos.DrawWireSphere(_rushTarget, 0.2f);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _landingDamageRadius);
        }
#endif
    }
}
