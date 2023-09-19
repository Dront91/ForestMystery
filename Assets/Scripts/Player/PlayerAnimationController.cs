using System.Collections.Generic;
using UnityEngine;

namespace MysteryForest
{
    public class PlayerAnimationController : MonoBehaviour
    {
        public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
        {
            public AnimationClipOverrides(int capacity) : base(capacity) { }

            public AnimationClip this[string name]
            {
                get { return this.Find(x => x.Key.name.Equals(name)).Value; }
                set
                {
                    int index = this.FindIndex(x => x.Key.name.Equals(name));
                    if (index != -1)
                        this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
                }
            }
        }
        public enum PlayerDirection
        {
            Right = 0,
            Left = 1,
            Up = 2,
            Down = 3
        }

        [SerializeField] private Fighter _player;
        [SerializeField] private AnimatorOverrideController _animatorOverrideController;
        [SerializeField] private PlayerAnimationAsset _playerAnimationAsset;
        private PlayerInventory _playerInventory;
        private Animator _animator;
        private string _baseMoveClip;
        private string _baseIdleClip;
        private string _baseAttackWithoutWeaponClip;
        private string _baseStickAttackClip;
        private string _baseSwordAttackClip;
        private string _baseBowAttackClip;
        private string _baseSlingshotAttackClip;
        private string _baseDeadClip;
        private PlayerDirection _currentDirection = PlayerDirection.Right;
        public PlayerDirection CurrentDirection => _currentDirection;
        protected AnimationClipOverrides _clipOverrides;
        private PlayerInventory.WeaponEquiped _currentWeapon; 
        


        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
            _animator.runtimeAnimatorController = _animatorOverrideController;
            _clipOverrides = new AnimationClipOverrides(_animatorOverrideController.overridesCount);
            _animatorOverrideController.GetOverrides(_clipOverrides);
            _playerInventory = GetComponent<PlayerInventory>();
            _currentWeapon = _playerInventory.WeaponEquipped;
            _playerInventory.OnWeaponTypeChange += UpdateWeaponClips;
            SetClips();
        }
        private void OnDestroy()
        {
            _playerInventory.OnWeaponTypeChange -= UpdateWeaponClips;
        }
        private void Update()
        {
            UpdateDirection();
            CheckMoveStatus();
        }
        private void SetClips()
        {
            _baseAttackWithoutWeaponClip = _playerAnimationAsset.WithoutWeaponAttackAnimClips[0].clip.name;
            _baseIdleClip = _playerAnimationAsset.IdleAnimClips[0].withoutWeaponClip.name;
            _baseMoveClip = _playerAnimationAsset.MoveAnimClips[0].withoutWeaponClip.name;
            _baseStickAttackClip = _playerAnimationAsset.StickAttackAnimClips[0].clip.name;
            _baseSwordAttackClip = _playerAnimationAsset.SwordAttackAnimClips[0].clip.name;
            _baseSlingshotAttackClip = _playerAnimationAsset.SlingshotAttackAnimClips[0].clip.name;
            _baseBowAttackClip = _playerAnimationAsset.BowAttackAnimClips[0].clip.name;
            _baseDeadClip = _playerAnimationAsset.DeadAnimClips[0].clip.name;
        }

        public void PlayAttackAnimation()
        {
            UpdateWeaponClips(); // Убрать после тестов
            switch (_currentWeapon)
            {
                case PlayerInventory.WeaponEquiped.Bow:
                    _animator.SetTrigger("Bow");
                    break;
                case PlayerInventory.WeaponEquiped.Sword:
                    _animator.SetTrigger("Sword");
                    break;
                case PlayerInventory.WeaponEquiped.Stick:
                    _animator.SetTrigger("Stick");
                    break;
                case PlayerInventory.WeaponEquiped.Slingshot:
                    _animator.SetTrigger("Slingshot");
                    break;
                case PlayerInventory.WeaponEquiped.Without:
                    _animator.SetTrigger("Without");
                    break;
            }
        }

        private void UpdateDirection()
        {
            var lastDir = _currentDirection;
           if(_player.MoveVector != Vector2.zero)
            {
                if (Mathf.Abs(_player.MoveVector.x) < Mathf.Abs(_player.MoveVector.y))
                {
                    if (_player.MoveVector.y >= 0) { _currentDirection = PlayerDirection.Up; }
                    if (_player.MoveVector.y < 0) { _currentDirection = PlayerDirection.Down; }
                }
                if (Mathf.Abs(_player.MoveVector.x) >= Mathf.Abs(_player.MoveVector.y))
                {
                    if (_player.MoveVector.x >= 0) { _currentDirection = PlayerDirection.Right; }
                    if (_player.MoveVector.x < 0) { _currentDirection = PlayerDirection.Left; }
                }
            }
            if(lastDir != _currentDirection) { UpdateDirectionClips(); }
            
        }
        private void UpdateDirectionClips()
        {
            _clipOverrides[_baseAttackWithoutWeaponClip] = _playerAnimationAsset.WithoutWeaponAttackAnimClips[(int)_currentDirection].clip;
            _clipOverrides[_baseStickAttackClip] = _playerAnimationAsset.StickAttackAnimClips[(int)_currentDirection].clip;
            _clipOverrides[_baseSwordAttackClip] = _playerAnimationAsset.SwordAttackAnimClips[(int)_currentDirection].clip;
            _clipOverrides[_baseBowAttackClip] = _playerAnimationAsset.BowAttackAnimClips[(int)_currentDirection].clip;
            _clipOverrides[_baseSlingshotAttackClip] = _playerAnimationAsset.SlingshotAttackAnimClips[(int)_currentDirection].clip;
            _clipOverrides[_baseDeadClip] = _playerAnimationAsset.DeadAnimClips[(int)_currentDirection].clip;

            switch(_currentWeapon)
            {
                    case PlayerInventory.WeaponEquiped.Bow:
                    _clipOverrides[_baseMoveClip] = _playerAnimationAsset.MoveAnimClips[(int)_currentDirection].withBowClip;
                    _clipOverrides[_baseIdleClip] = _playerAnimationAsset.IdleAnimClips[(int)_currentDirection].withBowClip;
                    break;
                    case PlayerInventory.WeaponEquiped.Sword:
                    _clipOverrides[_baseMoveClip] = _playerAnimationAsset.MoveAnimClips[(int)_currentDirection].withSwordClip;
                    _clipOverrides[_baseIdleClip] = _playerAnimationAsset.IdleAnimClips[(int)_currentDirection].withSwordClip;
                    break;
                    case PlayerInventory.WeaponEquiped.Stick:
                    _clipOverrides[_baseMoveClip] = _playerAnimationAsset.MoveAnimClips[(int)_currentDirection].withStickClip;
                    _clipOverrides[_baseIdleClip] = _playerAnimationAsset.IdleAnimClips[(int)_currentDirection].withStickClip;
                    break;
                     case PlayerInventory.WeaponEquiped.Slingshot:
                    _clipOverrides[_baseMoveClip] = _playerAnimationAsset.MoveAnimClips[(int)_currentDirection].withSlingshotClip;
                    _clipOverrides[_baseIdleClip] = _playerAnimationAsset.IdleAnimClips[(int)_currentDirection].withSlingshotClip;
                    break;
                     case PlayerInventory.WeaponEquiped.Without:
                    _clipOverrides[_baseMoveClip] = _playerAnimationAsset.MoveAnimClips[(int)_currentDirection].withoutWeaponClip;
                    _clipOverrides[_baseIdleClip] = _playerAnimationAsset.IdleAnimClips[(int)_currentDirection].withoutWeaponClip;
                    break;
            }
            _animatorOverrideController.ApplyOverrides(_clipOverrides);
        }
        private void CheckMoveStatus()
        {
            if(_player.MoveVector != Vector2.zero)
            _animator.SetTrigger("Move");
            else
                _animator.SetTrigger("Idle");
        }

        public void UpdateWeaponClips()
        {
            _currentWeapon = _playerInventory.WeaponEquipped;
            UpdateDirectionClips();
        }
        public void PlayPlayerAnimationDead()
        {
            UpdateDirectionClips();
            _animator.SetTrigger("Dead");
        }
    }

        
    
}
