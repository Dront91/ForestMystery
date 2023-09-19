using UnityEngine;
using UnityEngine.Timeline;

namespace MysteryForest
{
    public class HornetAI : AIControllerBase
    {
        public enum AIType
        {
            Idle,
            PatrolAndPursuit
        }

        [SerializeField] private AIType _behaviourType;
        [SerializeField] private AttackPoint _npcAttackPoint;
        [SerializeField] private PatrolPoint[] _patrolRoute;

        private Vector2 _movePoint;
        private Fighter _npc; 

        private int _nextPatrolPoint;
        private float _timeToAttack;

        protected override void Awake()
        {
            base.Awake();

            _npc = transform.GetComponent<Fighter>();
        }
        private void Start()
        {
            if (_behaviourType == AIType.PatrolAndPursuit && _patrolRoute.Length == 0)
                _behaviourType = AIType.Idle;
        }

        private void Update()
        {
            if (ActivItems.Invisibility || Invisibility)
            {
                return;
            }

            ActionIdle();

            ActionUpdateMovePoint();

            FindAttackTarget();

            ActionAttack();

            Flip();
        }

        private void ActionIdle()
        {
            _npc.MoveVector = Vector2.zero;
        }

        private void ActionUpdateMovePoint()
        {
            if (_attackTarget != null)
            {
                _movePoint = _attackTarget.transform.position;

                ActionMove();

                return;
            }

            if (_behaviourType != AIType.PatrolAndPursuit) return;

            bool isInsidePatrolPoint = Vector2.Distance(_patrolRoute[_nextPatrolPoint].transform.position,
                _npc.transform.position) < _patrolRoute[_nextPatrolPoint].Radius;

            if (isInsidePatrolPoint == true)
            {
                _nextPatrolPoint++;

                if (_nextPatrolPoint == _patrolRoute.Length)
                {
                    _nextPatrolPoint = 0;
                }
                _movePoint = _patrolRoute[_nextPatrolPoint].transform.position;
            }
            else
            {
                _movePoint = _patrolRoute[_nextPatrolPoint].transform.position;
            }
            ActionMove();
        }

        private void ActionMove()
        {
            Vector2 direction = _movePoint - (Vector2)transform.position;

            _npc.MoveVector = direction.normalized;


            if (_attackTarget != null && Vector2.Distance(_movePoint, (Vector2)transform.position) < _npcAttackPoint.AttackDistance)
            {
                _npc.MoveVector = Vector2.zero;

                _npcAttackPoint.transform.right = direction;
            }

            if (Vector2.Distance(_movePoint, (Vector2)transform.position) > _npcSightDistance)
            {
                _attackTarget = null;
            }
        }

        private void ActionAttack()
        {
            if (_timeToAttack < Time.time)
            {
                _npcAttackPoint.MeleeAttack();
                _timeToAttack = Time.time + _npc.AttackRate;
            }
        }

        protected override void Flip()
        {
            Vector2 direction = _movePoint - (Vector2)transform.position;

            if (direction.x >= 0)
            {
                _spriteRenderer.flipX = true;
            }
            else
            {
                _spriteRenderer.flipX = false;
            }
        }

        public void SetRoute(PatrolPoint[] route)
        {
            _patrolRoute = route;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(ActivItems.Invisibility || Invisibility)
                return;

            collision.transform.TryGetComponent(out Destructible _target);

            if (_target == null)
            {
                Physics2D.IgnoreLayerCollision(0, 7);
            }
        }

    }
}