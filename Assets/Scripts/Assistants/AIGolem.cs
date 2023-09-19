using Zenject;
using UnityEngine;
using UnityEngine.AI;
namespace MysteryForest
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody2D))]
    public class AIGolem : MonoBehaviour
    {
        private enum AIBehaviour
        {
            Stand,
            MoveToNextRoom,
            MoveToStandPoint,
            Follow
        }
        [SerializeField] private float _distanceToPlayer;
        [SerializeField] private float _offsetToStandPoint;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _maxMoveSpeed;
        [Inject] private RoomTransition _roomTransition;
        [Inject] private PlayerController _player;
        private BoxCollider2D _boxCollider2D;
        private Rigidbody2D _rigidbody;
        private RoomController _currentRoom;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private AIBehaviour _behaviourAI;
        private Vector2 _moveVector;
        private Vector2 _movePoint;
        private NavMeshAgent _agent;
        
        private bool IsEnterInClearRoom = false;
        private void Start()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
            _animator = transform.GetComponentInChildren<Animator>();
            _currentRoom = _roomTransition.CurrentRoom;
            _behaviourAI = AIBehaviour.Follow;
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
            _agent.enabled = true;
            _roomTransition.OnPlayerGoToGate += OnPlayerEnterRoom;
            AddListners();
        }
        private void OnDestroy()
        {
            _roomTransition.OnPlayerGoToGate -= OnPlayerEnterRoom;
            ResetListners();
            
        }
        private void Update()
        {
            UpdateMovePoint();
            Move();
            Flip();
        }
        private void FixedUpdate()
        {
            UpdateRigid();
        }

        private void UpdateMovePoint()
        {
            if (_behaviourAI == AIBehaviour.Follow)
            {
                _movePoint = _player.transform.position;
            }
            if (_behaviourAI == AIBehaviour.MoveToNextRoom )
            {
                _movePoint = _roomTransition.PlayerDestinationPoint.transform.position;
                _agent.enabled = false;
            }
            if (_behaviourAI == AIBehaviour.Stand)
            {
                _movePoint = _currentRoom.ViewPoint.transform.position;
                _agent.enabled = false;
            }
            if (_behaviourAI == AIBehaviour.MoveToStandPoint)
            {
                _movePoint = _currentRoom.ViewPoint.transform.position;
            }

        }
        private void Move()
        {
            if (_behaviourAI == AIBehaviour.Stand) 
            {
                _moveVector = Vector2.zero;
            }
            if (_behaviourAI == AIBehaviour.MoveToStandPoint)
            {
                Vector2 direction = _movePoint - (Vector2)transform.position;
                _moveVector = direction.normalized;
                if (Vector2.Distance(_movePoint, (Vector2)transform.position) < _offsetToStandPoint)
                {
                    _moveVector = Vector2.zero;
                    _behaviourAI = AIBehaviour.Stand;
                    _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                }
            }
            if (_behaviourAI == AIBehaviour.Follow )
            {
                
                if (Vector2.Distance(_movePoint, (Vector2)transform.position) > _distanceToPlayer)
                {
                    _agent.SetDestination(_movePoint);
                    
                }
                else { _agent.ResetPath(); }
                
            }
            if(_behaviourAI == AIBehaviour.MoveToNextRoom)
            {
                Vector2 direction = _movePoint - (Vector2)transform.position;
                _moveVector = direction.normalized;
                if(Vector2.Distance(_movePoint, (Vector2)transform.position) < _offsetToStandPoint)
                {
                    _moveVector = Vector2.zero;
                    if(IsEnterInClearRoom == false)
                    {
                        _behaviourAI = AIBehaviour.MoveToStandPoint;
                    }
                    else
                    {
                        _behaviourAI = AIBehaviour.Follow;
                        _agent.enabled = true;
                    }
                    _boxCollider2D.enabled = true;
                }
            }

        }
        private void OnPlayerEnterRoom(RoomController roomController)
        {
            IsEnterInClearRoom = false;
            ResetListners();
            _currentRoom = roomController;
            AddListners();
            _behaviourAI = AIBehaviour.MoveToNextRoom;
            _boxCollider2D.enabled = false;
        }
        private void OnAllEnemyInRoomDead()
        {
            _behaviourAI = AIBehaviour.Follow;
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            _agent.enabled = true;
        }
        private void OnClearRoomEnter()
        {
            IsEnterInClearRoom = true;
        }

        private void ResetListners()
        {
            _currentRoom.OnAllEnemyInRoomDead.RemoveListener(OnAllEnemyInRoomDead);
            _currentRoom.OnClearRoomEnter.RemoveListener(OnClearRoomEnter);
        }
        private void AddListners()
        {
            _currentRoom.OnAllEnemyInRoomDead.AddListener(OnAllEnemyInRoomDead);
            _currentRoom.OnClearRoomEnter.AddListener(OnClearRoomEnter); 
        }

        private void UpdateRigid()
        {
            _rigidbody.AddForce(new Vector2(_moveVector.x, _moveVector.y) * _moveSpeed);
            _rigidbody.AddForce(-_rigidbody.velocity * (_moveSpeed - _maxMoveSpeed));
        }
        private void Flip()
        {
           
            _animator.SetFloat("speed", _agent.velocity.magnitude);

            if (_agent.velocity.x >= 0)
            {
                _spriteRenderer.flipX = true;
               
            }
            else
            {
                _spriteRenderer.flipX = false;
               
            }
        }


    }
}
