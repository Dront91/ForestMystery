using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MysteryForest
{
    public class PlayerInventory : MonoBehaviour
    {
        public enum WeaponEquiped
        {
            Without,
            Stick,
            Bow,
            Slingshot,
            Sword
        }
        public int Keys { get; private set; }
        public Dictionary<BombAsset, int> Bombs { get; private set; }
        public WeaponAsset Weapon { get; private set; }
        public ActiveItemAsset ActiveItem { get; private set; }
        public List<PassiveItemAsset> PassiveItems { get; private set; }
        public BombAsset CurrentBomb { get; private set; }

        public int TotalHealthIncrease { get; private set; }
        public float TotalDamageIncrease { get; private set; }
        public float TotalAttackRangeIncrease { get; private set; }

        [Inject] private UICanvasManager _uICanvasManager;

        [SerializeField] private List<BombAsset> _bombTypes;
        [SerializeField] private WeaponEquiped _weaponEquiped;
        public WeaponEquiped WeaponEquipped => _weaponEquiped;
        [SerializeField] private WeaponAsset _stick;
        [SerializeField] private WeaponAsset _slingshot;

        private int _currentBombIndex;
        private int _bombTakeIndex;
        private WeaponAsset.WeaponType _lastWeaponType;

        public event Action OnPassiveItemAdded;
        private event Action OnPassiveItemRemoved;
        public event Action OnWeaponTypeChange;
        public Action OnKeyAdded;
        public Action OnBombAdded;
        public Action OnActiveItemAdded;

        private void Start()
        {
            GiveRandomWeapon();
            Initialize();
        }

        private void OnDestroy()
        {
            OnPassiveItemAdded -= CalculateStatChanges;
            OnPassiveItemRemoved -= CalculateStatChanges;
        }
        private void GiveRandomWeapon()
        {
            var roll = UnityEngine.Random.Range(0, 2);
            if (roll == 0)
            {
                SetWeapon(_stick);
            }
            else 
            {
                SetWeapon(_slingshot);
            }
        }

        public void AddKeys(int amount)
        {
            Keys += amount;
            OnKeyAdded?.Invoke();

            if (_uICanvasManager.TryGetComponent<StatesLevel>(out var statesLevel))
                statesLevel.UpdateTextKeys(Keys);
        }
        public void RemoveKeys(int amount)
        {
            if (Keys - amount < 0)
                return;

            Keys -= amount;

            if (_uICanvasManager.TryGetComponent<StatesLevel>(out var statesLevel))
                statesLevel.UpdateTextKeys(Keys);
        }

        public void AddBomb(BombAsset bomb, int count)
        {

            if (Bombs.ContainsKey(bomb))
            {
                for (int i = 0; i < _bombTypes.Count; i++)
                {
                    if (bomb.name == _bombTypes[i].name)
                    {
                        _bombTakeIndex = i;
                    }
                }

                Bombs[bomb] += count;
                OnBombAdded?.Invoke();

                if (_uICanvasManager.TryGetComponent<StatesLevel>(out var statesLevel))
                    statesLevel.UpdateTextBombs(Bombs[bomb], _bombTakeIndex);
            }
            else
            {
                Bombs[bomb] = count;
            }
        }

        public void RemoveBomb(BombAsset bomb, int count)
        {
            if (Bombs.ContainsKey(bomb))
            {
                Bombs[bomb] -= count;
                if (Bombs[bomb] < 0)
                {
                    Bombs[bomb] = 0;
                }

                if (_uICanvasManager.TryGetComponent<StatesLevel>(out var statesLevel))
                    statesLevel.UpdateTextBombs(Bombs[bomb], _currentBombIndex);
            }

        }

        public void BombAutoSwitch(BombAsset bomb)
        {
            if (Bombs[bomb] == 0)
            {
                int _index = 0;

                foreach (var _bomb in Bombs)
                {
                    if (_bomb.Value != 0)
                    {
                        _currentBombIndex = _index;
                        CurrentBomb = _bombTypes[_currentBombIndex];
                        break;
                    }
                    _index++;
                }
                if (_uICanvasManager.TryGetComponent<StatesLevel>(out var statesLevel))
                    statesLevel.UpdateBombs(_currentBombIndex);
            }
        }

        public void SwitchBomb()
        {
            _currentBombIndex = (_currentBombIndex + 1) % _bombTypes.Count;
            CurrentBomb = _bombTypes[_currentBombIndex];

            if (_uICanvasManager.TryGetComponent<StatesLevel>(out var statesLevel))
                statesLevel.UpdateBombs(_currentBombIndex);
        }

        public bool CheckBombAvailable(BombAsset bomb)
        {
            return Bombs.ContainsKey(bomb) && Bombs[bomb] > 0;
        }

        private void StatesLevelCheck(WeaponAsset.WeaponType weaponType)
        {
            if (_uICanvasManager.TryGetComponent<StatesLevel>(out var statesLevel))
                statesLevel.UpdateWeapons((int)weaponType);
        }

        public void SetWeapon(WeaponAsset weapon)
        {
            if (Weapon != null)
            {
                _lastWeaponType = Weapon.TypeWeapon;
            }
            Weapon = weapon;
            if (_lastWeaponType != Weapon.TypeWeapon) { OnWeaponTypeChange?.Invoke(); }
            if (Weapon.TypeWeapon == WeaponAsset.WeaponType.Stick) 
            { 
                _weaponEquiped = WeaponEquiped.Stick;
                StatesLevelCheck(WeaponAsset.WeaponType.Stick);
            }
            if (Weapon.TypeWeapon == WeaponAsset.WeaponType.Sword) {
                _weaponEquiped = WeaponEquiped.Sword;
                StatesLevelCheck(WeaponAsset.WeaponType.Sword);
            }
            if (Weapon.TypeWeapon == WeaponAsset.WeaponType.Bow) { 
                _weaponEquiped = WeaponEquiped.Bow;
                StatesLevelCheck(WeaponAsset.WeaponType.Bow);
            }
            if (Weapon.TypeWeapon == WeaponAsset.WeaponType.Slingshot) { 
                _weaponEquiped = WeaponEquiped.Slingshot;
                StatesLevelCheck(WeaponAsset.WeaponType.Slingshot);
            }
        }

        public void SetActiveItem(ActiveItemAsset activeItem)
        {
            ActiveItem = activeItem;
            OnActiveItemAdded?.Invoke();
        }

        public void AddPassiveItem(PassiveItemAsset passiveItem)
        {
            PassiveItems.Add(passiveItem);
            OnPassiveItemAdded?.Invoke();
        }

        public void RemovePassiveItem(PassiveItemAsset passiveItem)
        {
            PassiveItems.Remove(passiveItem);
            OnPassiveItemRemoved?.Invoke();
        }

        public bool CheckPassiveItem(PassiveItemAsset passiveItem)
        {
            return PassiveItems.Contains(passiveItem);
        }

        private void Initialize()
        {
            Bombs = new Dictionary<BombAsset, int>();
            PassiveItems = new List<PassiveItemAsset>();

            //if (Weapon == null) { _weaponEquiped = WeaponEquiped.Without; }

            foreach (BombAsset bomb in _bombTypes)
            {
                Bombs.Add(bomb, 0);
            }

            CurrentBomb = _bombTypes[0];

            OnPassiveItemAdded += CalculateStatChanges;
            OnPassiveItemRemoved += CalculateStatChanges;

            if (_uICanvasManager.TryGetComponent<StatesLevel>(out var statesLevel))
                statesLevel.Initiliaze();

        }

        private void CalculateStatChanges()
        {
            TotalHealthIncrease = 0;
            TotalDamageIncrease = 0;
            TotalAttackRangeIncrease = 0;

            foreach (PassiveItemAsset passiveItem in PassiveItems)
            {
                TotalHealthIncrease += passiveItem.MaxHealthIncrease;
                TotalDamageIncrease += passiveItem.DamageIncrease;
                TotalAttackRangeIncrease += passiveItem.AttackRangeIncrease;
            }
        }
    }
}