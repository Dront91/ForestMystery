using UnityEngine;

namespace MysteryForest
{
    [RequireComponent(typeof(SoundHook))]
    public class PlayerSoundController : BaseEnemySoundController
    {
        private PlayerFighter _playerFighter;
        private PlayerController _playerController;
        private PlayerInventory _playerInventory;

        protected override void Start()
        {
            base.Start();
            _playerFighter = GetComponentInParent<PlayerFighter>();
            _playerController = GetComponentInParent<PlayerController>();
            _playerInventory = GetComponentInParent<PlayerInventory>();
            AddListeners();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            RemoveListeners();
        }
        private void RemoveListeners()
        {
            _playerFighter.DamageTaken -= OnDamageTaken;
            _playerFighter.RestoreHealth -= OnHealthRestore;
            _playerController.OnLoseScreenStart -= OnLoseScreenStart;
            _playerController.PlayerAttack -= OnPlayerAttack;
            _playerController.PlayerThrowBomb -= OnPlayerThrowBomb;
            _playerInventory.OnActiveItemAdded -= OnItemPickUp;
            _playerInventory.OnBombAdded -= OnItemPickUp;
            _playerInventory.OnKeyAdded -= OnItemPickUp;
            _playerInventory.OnPassiveItemAdded -= OnItemPickUp;
        }
        private void AddListeners()
        {
            _playerFighter.DamageTaken += OnDamageTaken;
            _playerFighter.RestoreHealth += OnHealthRestore;
            _playerController.OnLoseScreenStart += OnLoseScreenStart;
            _playerController.PlayerAttack += OnPlayerAttack;
            _playerController.PlayerThrowBomb += OnPlayerThrowBomb;
            _playerInventory.OnActiveItemAdded += OnItemPickUp;
            _playerInventory.OnBombAdded += OnItemPickUp;
            _playerInventory.OnKeyAdded += OnItemPickUp;
            _playerInventory.OnPassiveItemAdded += OnItemPickUp;
        }

        private void OnDamageTaken()
        {
            _audioClip = Sound.PlayerResiveDamage;
            PlaySound();
        }
        protected override void PlayStepSound()
        {
            base.PlayStepSound();
            _audioClip = Sound.PlayerSteps;
            PlaySound();

        }
        private void OnLoseScreenStart()
        {
            _audioClip = Sound.LoseScreenSound;
            PlaySound();
        }
        private void OnHealthRestore()
        {
            _audioClip = Sound.PlayerPotionUse;
            PlaySound();
        }

        private void OnPlayerAttack(PlayerInventory.WeaponEquiped weaponEquiped)
        {
            switch(weaponEquiped)
            {
                case PlayerInventory.WeaponEquiped.Sword:
                    _audioClip = Sound.PlayerAttackSword;
                    break;
                case PlayerInventory.WeaponEquiped.Stick:
                    _audioClip = Sound.PlayerAttackSword;
                    break;
                case PlayerInventory.WeaponEquiped.Bow:
                    _audioClip = Sound.PlayerAttackBow;
                    break;
                case PlayerInventory.WeaponEquiped.Slingshot:
                    _audioClip = Sound.PlayerAttackSlingshot;
                    break;
                case PlayerInventory.WeaponEquiped.Without:
                    _audioClip = Sound.PlayerAttackWithoutWeapon;
                    break;
            }
            PlaySound();
        }

        private void OnPlayerThrowBomb()
        {
            _audioClip = Sound.PlayerThrowBomb;
            PlaySound();
        }

        private void OnItemPickUp()
        {
            _audioClip = Sound.PlayerItemPickUp;
            PlaySound();
        }
    }
}
