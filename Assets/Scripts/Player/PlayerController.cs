using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace MysteryForest
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerFighter _player;
        [SerializeField] private AttackPoint _attackPoint;
        [SerializeField] private GameObject _arrowPrefab;
        [SerializeField] private GameObject _stonePrefab;
        [SerializeField] private GameObject _spawnPoint;
        [SerializeField] private float _offset;
        [SerializeField] private Collider2D _moveCollider;
        public Collider2D MoveCollider => _moveCollider;
        [Inject] private LevelSequenceController _levelSequenceController;
        [Inject] private SoundPlayer _soundPlayer;
        [Inject] private DiContainer _diContainer;
        public Action OnLoseScreenStart;
        public Action PlayerThrowBomb;
        public Action<PlayerInventory.WeaponEquiped> PlayerAttack;
        private PlayerAnimationCatcher _playerAnimationCatcher;
        private SpriteRenderer _spriteRenderer;
        private PlayerAnimationController _playerAnimationController;
        public PlayerAnimationController PlayerAnimationController => _playerAnimationController;
        private PlayerInventory _playerInventory;
        private PlayerInputHandler _playerInput;
        private Coroutine _moveCoroutine;
        private float _timeToAttack;
        private float _offsetY = 0f;
        [Inject] private UICanvasManager _uICanvasManager;

        private void Start()
        {
            _playerAnimationController = GetComponent<PlayerAnimationController>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _playerAnimationCatcher = GetComponentInChildren<PlayerAnimationCatcher>();
            _playerInput = GetComponent<PlayerInputHandler>();
            _playerInventory = GetComponent<PlayerInventory>();
            _player.OnDeath += OnPlayerDead;
            _playerAnimationCatcher.OnAnimationAttackEnd += RestrictPlayerActions;
            _playerAnimationCatcher.OnAnimationThrow += InstantiateProjectile;
            _playerAnimationCatcher.OnAnimationDeadEnd += OnPlayerAnimationDeadEnd;
        }
        private void FixedUpdate()
        {
            HandlePlayerActions();
        }

        private void OnDestroy()
        {
            _player.OnDeath -= OnPlayerDead;
            _playerAnimationCatcher.OnAnimationAttackEnd -= RestrictPlayerActions;
            _playerAnimationCatcher.OnAnimationThrow -= InstantiateProjectile;
            _playerAnimationCatcher.OnAnimationDeadEnd -= OnPlayerAnimationDeadEnd;

        }
        private void Move(Vector2 velocity)
        {
            _player.MoveVector = velocity;
        }

        //2 Button
        private void SwitchCurrentBomb(bool isSwitched)
        {
            if (isSwitched)
            {
                _playerInventory.SwitchBomb();
               // print(_playerInventory.CurrentBomb + " / " + _playerInventory.Bombs[_playerInventory.CurrentBomb]);
                _playerInput.ResetInputState();
            }
        }

        //R Button
        private void ThrowBomb(bool isThrown)
        {
            if (isThrown)
            {
                if (!_playerInventory.CheckBombAvailable(_playerInventory.CurrentBomb))
                    return;

                PlayerThrowBomb?.Invoke();

                _playerInventory.RemoveBomb(_playerInventory.CurrentBomb, 1);
                _diContainer.InstantiatePrefab(_playerInventory.CurrentBomb.BombProjectile, transform.position, Quaternion.identity, null);

                _playerInventory.BombAutoSwitch(_playerInventory.CurrentBomb);

                _playerInput.ResetInputState();
            }
        }

        //F Button
        private void UseItem(bool isUsed)
        {
            if (isUsed)
            {
                //test purposes only
                _playerInventory.RemovePassiveItem(_playerInventory.PassiveItems[^1]);
                //print("used");
                _playerInput.ResetInputState();
            }
        }
        //LKM
        private void Attack(bool isAttacked)
        {
            if (_timeToAttack < Time.time)
            {
                if (isAttacked)
                {
                    _playerAnimationController.PlayAttackAnimation();
                    PlayerAttack?.Invoke(_playerInventory.WeaponEquipped);
                    RestrictPlayerActions(false);
                    if (_playerInventory.WeaponEquipped == PlayerInventory.WeaponEquiped.Stick
                        || _playerInventory.WeaponEquipped == PlayerInventory.WeaponEquiped.Without
                        || _playerInventory.WeaponEquipped == PlayerInventory.WeaponEquiped.Sword)
                    {
                        _attackPoint.MeleeAttack();
                    }
                    _playerInput.ResetInputState();
                    _timeToAttack = Time.time + _player.AttackRate;
                }
            }
        }
        private void InstantiateProjectile(string projectileType)
        {
            var direction = Vector2.zero;
            float angle = 0f;
            switch(_playerAnimationController.CurrentDirection)
            {
                case PlayerAnimationController.PlayerDirection.Right:
                    direction = Vector2.right;
                    angle = 90f;
                    break;
                case PlayerAnimationController.PlayerDirection.Left:
                    direction = Vector2.left;
                    angle = 270f;
                    break;
                case PlayerAnimationController.PlayerDirection.Up:
                    direction = Vector2.up;
                    angle = 180f;
                    break;
                case PlayerAnimationController.PlayerDirection.Down:
                    direction = Vector2.down;
                    angle = 0f;
                    break;
            }
            _spawnPoint.transform.position = (Vector2)transform.position + new Vector2(0, _offsetY) + direction * _offset;
            var endPosition = (Vector2)_spawnPoint.transform.position + direction * _player.PlayerRangeAttackDistance;
            if(projectileType == "Arrow")
            {
                _ = gameObject.Instantiate(_arrowPrefab, _spawnPoint.transform.position, Quaternion.identity, _player.Collider, endPosition, _player.Damage, _player.ArrowPushForce, angle, _soundPlayer);
            }
            if(projectileType == "Stone")
            {
                _ = gameObject.Instantiate(_stonePrefab, _spawnPoint.transform.position, Quaternion.identity, _player.Collider, endPosition, _player.Damage, _player.SlingshotPushForce, angle, _soundPlayer);
            }
        }

        private void OnPlayerDead(Destructible des)
        {
            _playerAnimationController.PlayPlayerAnimationDead();
            Destroy(_player);
        }
        private void OnPlayerAnimationDeadEnd()
        {
            _uICanvasManager.SwitchCanvas(CanvasType.LoseScreen);
            OnLoseScreenStart?.Invoke();
            _levelSequenceController.InvokeLoseLevel();
            Destroy(gameObject);
        }

        private void HandlePlayerActions()
        {
            if (_playerInput.enabled == true)
            {
                Move(_playerInput.MovementInput);
            }
            Attack(_playerInput.AttackInput);
            ThrowBomb(_playerInput.ThrowBombInput);
            SwitchCurrentBomb(_playerInput.SwitchBombInput);
            UseItem(_playerInput.UseItemInput);
        }
        public IEnumerator MovePlayer(Vector2 target, float transitionSeconds)
        {
            RestrictPlayerActions(false);

            float distance = Vector2.Distance(_player.transform.position, target);
            float speed = distance / transitionSeconds;

            float elapsedTime = 0f;
            Vector2 startPosition = _player.transform.position;

            while (elapsedTime < transitionSeconds)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / transitionSeconds);
                _player.transform.position = Vector2.Lerp(startPosition, target, t * speed);

                yield return null;
            }

            RestrictPlayerActions(true);
        }
        private void RestrictPlayerActions(bool state)
        {

            _playerInput.enabled = state;
            _moveCollider.enabled = state;
            _player.MoveVector = Vector2.zero;
        }


    }
}