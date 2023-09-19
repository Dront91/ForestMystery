using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System;

namespace MysteryForest
{
    [RequireComponent(typeof(Rigidbody2D), typeof(NavMeshAgent), typeof(Destructible))]
    public class AIHedgehog : AIControllerBase
    {
        private enum AIBehaviour
        {
            Idle,
            AttackPosition,
            RollPrepare,
            Roll,
            Unfold,
            Stunned
        }
      
        [SerializeField] private float _rollSpeed;
        [SerializeField] private float _rollDistance;
        [SerializeField] private float _rollAttackTimeRate;
        [SerializeField] private float _bounceForce;
        [SerializeField] private float _stunTime;
        [SerializeField] private float _offsetAttackDistance;
        [SerializeField] private int _damage;
        [SerializeField] private float _pushForce;
        [SerializeField] private float _navMeshSpeed = 1;

        public Action OnStunStart;
        public Action OnStunEnd;
        public Action OnWallCrash;
        public Action OnRollStart;
        public Action OnRollEnd;
        public Action OnRushStart;

        private LayerMask _enemyLayerMask;
        private AIBehaviour _currentBehaviour;
        private Animator _animator;
        private Collider2D _targetCollider;
        private Rigidbody2D _rigidbody2D;
        private Destructible _destructible;
        private HedgehogAnimationCatcher _animationEventCatcher;
        private RaycastHit2D _hit;
        private NavMeshAgent _agent;
        private Vector2 _target;

        public float NavMeshSpeed => _navMeshSpeed;
        public NavMeshAgent Agent => _agent;

        private int _dir;
        private float _timeToRoll;
        private float _timeToStun;

        override protected void Awake()
        {
            base.Awake();

            _rigidbody2D = GetComponent<Rigidbody2D>();
            _destructible = GetComponent<Destructible>();
            _animator = GetComponentInChildren<Animator>();

            _animationEventCatcher = GetComponentInChildren<HedgehogAnimationCatcher>();
            _animationEventCatcher.OnAnimationJumpFoldEnd += OnRollPrepareEnd;
            _animationEventCatcher.OnAnimationUnfoldEnd += OnUnfoldEnd;

            _enemyLayerMask = LayerMask.GetMask("Enemy");

        }
        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();

            _agent.speed = _navMeshSpeed;

            _agent.updateRotation = false;
            _agent.updateUpAxis = false;

            ChangeBehaviour(AIBehaviour.Idle);
        }

        
        private void Update()
        {
            if (ActivItems.Invisibility || Invisibility)
            {
                return;
            }

            if (_currentBehaviour == AIBehaviour.Idle)
            {
                FindAttackTarget();
            }
            if (_currentBehaviour == AIBehaviour.AttackPosition)
            {
                Flip();
                MoveToAttackPosition();                
                CheckForTargetInPosition();
            }
            if (_currentBehaviour == AIBehaviour.Roll)
            {
                RollToTarget();
            }
        }

        private void OnDestroy()
        {
            _animationEventCatcher.OnAnimationJumpFoldEnd -= OnRollPrepareEnd;
            _animationEventCatcher.OnAnimationUnfoldEnd -= OnUnfoldEnd;
        }

        protected override void Flip()
        {           
            _animator.SetFloat("speed", _agent.velocity.magnitude);
            if (_agent.velocity.x > 0)
            {
                _spriteRenderer.flipX = true;
                _dir = 1;
            }
            else
            {
                _spriteRenderer.flipX = false;
                _dir = -1;
            }
        }

        private void OnRollPrepareEnd()
        {
            OnRollStart?.Invoke();
            ChangeBehaviour(AIBehaviour.Roll);
        }

        private void OnUnfoldEnd()
        {
            _rigidbody2D.velocity = Vector3.zero;
            if(_animator.GetBool("stunned"))
            {
                OnStunStart?.Invoke();
                StartCoroutine(StunnedState());
                ChangeBehaviour(AIBehaviour.Stunned);
            }else
            {
                ChangeBehaviour(AIBehaviour.Idle);
            }
        }

        private void ChangeBehaviour(AIBehaviour behaviour)
        {
            _currentBehaviour = behaviour;
        }

        protected override void FindAttackTarget()
        {
            if(_attackTarget != null) ChangeBehaviour(AIBehaviour.AttackPosition);

            _npcSight = Physics2D.OverlapCircle(transform.position, _npcSightDistance, _targetLayerMask);

            if (_npcSight)
            {
                if (_npcSight.TryGetComponent(out _attackTarget))
                {
                    _targetCollider = _attackTarget.GetComponent<Collider2D>();
                    ChangeBehaviour(AIBehaviour.AttackPosition);
                }
            }
        }
        private void MoveToAttackPosition()
        {
            if (_attackTarget == null)
                return;

            _target.y = _attackTarget.transform.position.y;

            if (_attackTarget.transform.position.x - transform.position.x < 0)
                _target.x = _attackTarget.transform.position.x + _offsetAttackDistance;
            else
                _target.x = _attackTarget.transform.position.x - _offsetAttackDistance;

            _agent.enabled = true;
            _agent.SetDestination(_target);
        }

        private void CheckForTargetInPosition()
        {
            if (_timeToRoll > Time.time || _targetCollider == null) return;

            _hit = Physics2D.Raycast(transform.position, new Vector2(_dir, 0), _rollDistance);

            if (_hit.collider == _targetCollider)
            {
                _agent.enabled = false;
                _destructible.ChangePushable(false);
                OnRushStart?.Invoke();
                _animator.SetTrigger("rush");
                ChangeBehaviour(AIBehaviour.RollPrepare);
            }
            
        }
        private void RollToTarget()
        {
            transform.Translate(_dir * _rollSpeed * Time.deltaTime * Vector2.right);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_currentBehaviour != AIBehaviour.Roll) return;
            OnRollEnd?.Invoke();
            _animator.SetTrigger("crash");
            _timeToRoll = Time.time + _rollAttackTimeRate;

            _destructible.ChangePushable(true);
            Vector2 attackPoint = new Vector2(collision.transform.position.x, transform.position.y);
            _destructible.TakeDamage(0, _bounceForce, attackPoint);

            OnWallCrash?.Invoke();

            ChangeBehaviour(AIBehaviour.Unfold);
            if (1 << collision.gameObject.layer == _targetLayerMask || 1 << collision.gameObject.layer == _enemyLayerMask)
            {
                _animator.SetBool("stunned", false);

                if(collision.transform.TryGetComponent(out Destructible _target))
                    _target.TakeDamage(_damage, _pushForce, transform.position);
            }
            else
            {
                _destructible.TakeDamage(_damage , 0 , transform.position);
                _animator.SetBool("stunned", true);               
            }
        }
        private IEnumerator StunnedState()
        {
            while (_timeToStun < _stunTime)
            {
                _timeToStun += Time.deltaTime;
                yield return null;
            }
            _timeToStun = 0;
            OnStunEnd?.Invoke();
            _animator.SetTrigger("stunEnd");
            ChangeBehaviour(AIBehaviour.Idle);
        }
#if UNITY_EDITOR
        override protected void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + _rollDistance * _dir, transform.position.y));
        }
#endif
    }
}

