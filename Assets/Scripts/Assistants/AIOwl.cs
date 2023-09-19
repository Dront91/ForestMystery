using Zenject;
using UnityEngine;
using System;

namespace MysteryForest
{
    public class AIOwl : MonoBehaviour
    {
        private enum AIBehaviour
        {
            Stand,
            Fly,
            Landing,
            Takeoff
        }
        public Action<float> PlayStepSound;
        [SerializeField] private Vector2 _startPoint;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _glideOffsetX;
        [SerializeField] private float _glideOffsetY;
        [SerializeField] private float _flyAwayOffsetX;
        [Inject] private RoomTransition _roomTransition;
        private float _saveSpeed;
        private float _stepCycle;
        private Vector2 _currentMovePoint;
        private Vector2 _currentLandingPoint;
        private Vector2 _moveVector;
        private Vector2 _flyAwayPoint;
        private Vector2 _glidePathPoint;
        private BoxCollider2D _boxCollider2D;
        private AIBehaviour _behaviourType;
        private SpriteRenderer _spriteRenderer;
        private bool _moveNextRoom;
        private bool _isFlyAway;
        private Animator _animator;
        private OwlAnimationCatcher _owlAnimationCatcher;
        private RoomController _currentRoomController;
       
        private  void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            _owlAnimationCatcher = GetComponentInChildren<OwlAnimationCatcher>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
            _roomTransition.OnPlayerGoToGate += OnPlayerEnterRoom;
            SetStartSettings();
           // _owlAnimationCatcher.OnLandingEnd += OnLandingAnimationEnd;
            _owlAnimationCatcher.OnTakeOffEnd += OnTakeOffPrepareAnimationEnd;
        }
        private void SetStartSettings()
        {
            _saveSpeed = _moveSpeed;
            transform.position = _startPoint;
            _currentLandingPoint = _startPoint;
            _boxCollider2D.enabled = false;
            _behaviourType = AIBehaviour.Stand;
            _moveNextRoom = true;
        }

        private void OnDestroy()
        {
            _roomTransition.OnPlayerGoToGate -= OnPlayerEnterRoom;
           // _owlAnimationCatcher.OnLandingEnd -= OnLandingAnimationEnd;
            _owlAnimationCatcher.OnTakeOffEnd -= OnTakeOffPrepareAnimationEnd;
        }
        private void Update()
        {
            CheckBehaviour();
            Flip();
            CheckMovementStatus();
        }
        private void OnTakeOffPrepareAnimationEnd()
        {
            ChangeBehavior(AIBehaviour.Takeoff);
        }
        public void FlyAwayFromPlayer(Vector2 playerPosition)
        {
            CalculatePoints(playerPosition, false);
            _isFlyAway = true;
            _animator.SetTrigger("PrepareTakeOff");
        }
        private void OnPlayerEnterRoom(RoomController nextRoom)
        {
            if (nextRoom.IsOwlRequired != true || nextRoom.OwlStandPoint == null) return;
            UpdateLandingPoint(nextRoom.OwlStandPoint.transform.position);
            if(_currentRoomController != null)
            {
                _currentRoomController.OnAllEnemyInRoomDead.RemoveListener(OnAllEnemyDead);
            }
            _currentRoomController = nextRoom;
            _currentRoomController.OnAllEnemyInRoomDead.AddListener(OnAllEnemyDead);
           // _animator.SetTrigger("PrepareTakeOff");
        }
        private void OnAllEnemyDead()
        {
            _animator.SetTrigger("PrepareTakeOff");
        }
        private void CheckBehaviour()
        {
            switch (_behaviourType)
            {
                case AIBehaviour.Stand:
                    _moveVector = Vector2.zero;
                    _boxCollider2D.enabled = true;
                    _spriteRenderer.sortingOrder = 0;
                    break;
                case AIBehaviour.Takeoff:
                    MoveToPoint(_glidePathPoint);
                    if(CheckDestinationReached())
                    {
                        _moveSpeed = _saveSpeed;
                        ChangeBehavior(AIBehaviour.Fly);
                        _animator.SetTrigger("Fly");
                    }
                    _boxCollider2D.enabled = false;
                    _spriteRenderer.sortingOrder = 11;
                    break;
                case AIBehaviour.Landing:
                    MoveToPoint(_currentLandingPoint);
                    if (CheckDestinationReached())
                    {
                        ChangeBehavior(AIBehaviour.Stand);
                        _animator.SetTrigger("AfterLandingAnimation");
                    }
                    break;
                case AIBehaviour.Fly:
                    if (_moveNextRoom)
                    {
                        MoveToPoint(_glidePathPoint);
                        if (CheckDestinationReached())
                        {
                            _saveSpeed = _moveSpeed;
                            _moveSpeed /= 2;
                            ChangeBehavior(AIBehaviour.Landing);
                            _animator.SetTrigger("StartLanding");
                        }
                    }
                    else
                    {
                        if (_isFlyAway == true)
                        {
                            MoveToPoint(_flyAwayPoint);
                            if (CheckDestinationReached())
                            {
                                _isFlyAway = false;
                            }
                        }
                        else 
                        {
                            MoveToPoint(_glidePathPoint);
                            if (CheckDestinationReached())
                            {
                                _saveSpeed = _moveSpeed;
                                _moveSpeed /= 2;
                                ChangeBehavior(AIBehaviour.Landing);
                                _animator.SetTrigger("StartLanding");
                            }
                        }
                    }
                    break;
            }
        }
        private void UpdateLandingPoint(Vector2 position)
        {
            _currentLandingPoint = position;
            CalculatePoints(_currentLandingPoint, true);
        }
        private void ChangeBehavior(AIBehaviour type)
        {
            _behaviourType = type;
        }
        private void MoveToPoint(Vector2 currentMovePoint)
        {
            _currentMovePoint = currentMovePoint;
            Vector2 direction = _currentMovePoint - (Vector2)transform.position;
            _moveVector = direction.normalized;
            transform.Translate(_moveVector * _moveSpeed / 100);
        }
        private bool CheckDestinationReached()
        {
            if (Vector2.Distance(transform.position, _currentMovePoint) < 0.01f)
                return true;
            else
                return false;

        }
        protected virtual void Flip()
        {
            if (_behaviourType == AIBehaviour.Stand) return;
            if (transform.position.x - _currentMovePoint.x < 0)
            {
                _spriteRenderer.flipX = true;
            }
            else
            {
                _spriteRenderer.flipX = false;
            }
        }
        private void CalculatePoints(Vector2 landingPoint, bool isLandingPointUpdate)
        {
            if (isLandingPointUpdate)
            {
                _moveNextRoom = true;
                if (transform.position.x > landingPoint.x)
                {
                    _glidePathPoint = new Vector2(landingPoint.x + _glideOffsetX, landingPoint.y + _glideOffsetY);
                    _flyAwayPoint = new Vector2(_glidePathPoint.x + _flyAwayOffsetX, _glidePathPoint.y);
                }
                else
                {
                    _glidePathPoint = new Vector2(landingPoint.x - _glideOffsetX, landingPoint.y + _glideOffsetY);
                    _flyAwayPoint = new Vector2(_glidePathPoint.x - _flyAwayOffsetX, _glidePathPoint.y);
                }
            }
            else
            {
                _moveNextRoom = false;
                if(landingPoint.x < _currentLandingPoint.x)
                {
                    _glidePathPoint = new Vector2(_currentLandingPoint.x + _glideOffsetX, _currentLandingPoint.y + _glideOffsetY);
                    _flyAwayPoint = new Vector2(_glidePathPoint.x + _flyAwayOffsetX, _glidePathPoint.y);
                }
                else
                {
                    _glidePathPoint = new Vector2(_currentLandingPoint.x - _glideOffsetX, _currentLandingPoint.y + _glideOffsetY);
                    _flyAwayPoint = new Vector2(_glidePathPoint.x - _flyAwayOffsetX, _glidePathPoint.y);
                }
            }
        }
        private void CheckMovementStatus()
        {
            AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("Idle") == false || info.IsName("AfterLanding"))
            {
                _stepCycle += _moveSpeed * Time.deltaTime;
            }
            PlayStepSound?.Invoke(_stepCycle);
        }
    }
}
