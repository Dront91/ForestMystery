using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MysteryForest
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class Gate : MonoBehaviour
    {
        [SerializeField] private Gate _connectedGate;
        [SerializeField] private GameObject _playerSpawnPoint;
        [SerializeField] private float _gateOpeningSpeed = 0.01f;

        [Inject] private PlayerController _player;

        public Action<Gate> OnPlayerGateEnter;
        public Gate ConnectedGate => _connectedGate;
        public GameObject PlayerSpawnPoint => _playerSpawnPoint;
        public bool IsOpen => _isOpened;

        private bool _isOpened = false;

        private bool _isPlayerArrive = false;

        private CircleCollider2D _circleCollider;
        private Collider2D _playerCollider2D;
        private SpriteRenderer _circleRender;

        private float _coordX = 0.3f;

        private void Awake()
        {
            _playerCollider2D = _player.MoveCollider;
            _circleCollider = GetComponent<CircleCollider2D>();
            _circleRender = GetComponentInChildren<SpriteRenderer>();
            _coordX = _circleRender.transform.localScale.x;
        }
        private void Start()
        {
            _circleRender.transform.localScale = Vector3.zero;
            _circleCollider.isTrigger = true;  
            
            if (_connectedGate != null)
                _connectedGate._connectedGate = this;

            if (_isOpened)
                OpenGate();           
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision == _playerCollider2D)
            {
                if (_isPlayerArrive)
                {
                    _isPlayerArrive = false;
                    return;
                }
                if (!_isOpened) return;

                _connectedGate._isPlayerArrive = true;
                OnPlayerGateEnter?.Invoke(this);                                   
            }

        }
        public void OpenGate()
        {
            _isOpened = true;           
            StartCoroutine(GateOpenView());
        }
        
        public IEnumerator GateOpenView()
        {
            while (_circleRender.transform.localScale.x < _coordX)
            {
                _circleRender.transform.localScale += new Vector3(_gateOpeningSpeed, _gateOpeningSpeed * 2, 0);
                yield return null;
            }
        }
        
    }
}
