using System;
using System.Collections;
using UnityEngine;
using Zenject;


namespace MysteryForest
{
    public class RoomTransition : MonoBehaviour
    {
        [SerializeField] private RoomController _startRoom;
        [SerializeField] private float _transitionSpeedPlayer = 1.5f;
        [SerializeField] private float _transitionSpeedCamera = 5f;
        public RoomController StartRoom => _startRoom;
        [SerializeField] private float _transitionSeconds = 1f;

        [Inject] private PlayerController _player;
        private GameObject _playerDestinationPoint;
        public GameObject PlayerDestinationPoint => _playerDestinationPoint;
        private RoomController _currentRoom;
        public RoomController CurrentRoom => _currentRoom;
        private RoomController _nextRoom;
        private float _transitionProgress;

        public Action<RoomController> OnPlayerGoToGate;

        private void Awake()
        {
            _currentRoom = _startRoom;
            Camera.main.transform.position = _currentRoom.ViewPoint.transform.position;

        }
        private void Start()
        {
            _currentRoom.Enter();
        }
        public void SetDestinationRoom(Gate destinationGate)
        {
            _nextRoom = destinationGate.ConnectedGate.GetComponentInParent<RoomController>();
            if (destinationGate.ConnectedGate.PlayerSpawnPoint == null)
                _playerDestinationPoint = destinationGate.ConnectedGate.gameObject;
            else
                _playerDestinationPoint = destinationGate.ConnectedGate.PlayerSpawnPoint;
            OnPlayerGoToGate?.Invoke(_nextRoom);
            StartCoroutine(_player.MovePlayer(_playerDestinationPoint.transform.position, _transitionSeconds / _transitionSpeedPlayer));
            //_player.enabled = false;
            //_player.GetComponentInChildren<Fighter>().MoveVector = Vector2.zero;

            StartCoroutine(TransitRoom());
        }
        IEnumerator TransitRoom()
        {
            while (_transitionProgress < 1f)
            {
                _transitionProgress = Mathf.Clamp01(_transitionProgress + Time.deltaTime / _transitionSeconds);

                float blend = Mathf.SmoothStep(0f, 1f, _transitionProgress)* _transitionSpeedCamera;

                Camera.main.transform.position = Vector3.Lerp(_currentRoom.ViewPoint.transform.position,
                                                              _nextRoom.ViewPoint.transform.position, blend);
                yield return null;
            }

            //_player.transform.position = _playerDestinationPoint.transform.position;
            //_player.enabled = true;
           
            _currentRoom.Leave();
            _currentRoom = _nextRoom;
            _nextRoom = null;
            _transitionProgress = 0f;
            _currentRoom.Enter();
        }
    }
}
