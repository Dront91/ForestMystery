using System.Collections.Generic;
using TowerDefense;
using UnityEngine;
using Zenject;


namespace MysteryForest
{
    public class Chest : Entity
    {
        [Range(0f, 1f)]
        [SerializeField] private float _spawnChance;
        [SerializeField] private bool _isKeyRequired;
        [SerializeField] private SoundHook _UseKeySoundHook;
        [SerializeField] private SoundHook _OpenChestSoundHook;
        [SerializeField] private GameObject _impactEffect;
        [SerializeField] private float _spawnAreaOffset;
        [Inject] private DiContainer _diContainer;
        public float SpawnChance => _spawnChance;

        private RoomController _roomController;
        private Animator _animator;
        private bool _isOpened;
        private List<Entity> _dropList;
        private ChestSpawnArea _spawnLootArea;
        private Vector2 spawnPoint;
        private int _capacity;
        private Entity _badlyItem;
        private bool _isAnyItemDrop = false;
        

        public override void Start()
        {
            base.Start();
            _roomController = GetComponentInParent<RoomController>();
            _roomController.OnPlayerLeaveRoom += DestroyChest;
        }
        private void OnDestroy()
        {
            _roomController.OnPlayerLeaveRoom -= DestroyChest;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.TryGetComponent(out PlayerInventory player))
                return;
            if (_isOpened == true)
                return;
            if (!_isKeyRequired)
                OpenChest();

            if (_isKeyRequired && player.Keys <= 0)
                return;
            if(_isKeyRequired == false)
            {
                OpenChest();
                return;
            }
            _UseKeySoundHook.Play();
            player.RemoveKeys(1);
            OpenChest();
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!collision.TryGetComponent(out PlayerInventory player))
                return;
            var newSpawnPosition = ((player.transform.position - transform.position) / Vector2.Distance(player.transform.position, transform.position)) * _spawnAreaOffset;
            _spawnLootArea.MoveAwayFromPlayer(newSpawnPosition);
        }

        public override void ApplyPreferences(EntityAsset asset)
        {
            base.ApplyPreferences(asset);
            if (EntityAsset is ChestAsset)
            {
                _animator = GetComponentInChildren<Animator>();
                _isOpened = false;
                _spawnLootArea = GetComponentInChildren<ChestSpawnArea>();
                _capacity = (EntityAsset as ChestAsset).ChestCapacity;
                SetDropList();
            }
            else
            {
                Debug.Log("Uncorrect Asset, check asset type, only ChestAsset requared!");
            }
        }
        private void OpenChest()
        {
            if (_isOpened)
                return;
            _animator.enabled = true;
            Invoke(nameof(DropItems), 0.25f);
            _isOpened = true;
            Invoke(nameof(DestroyChest), 3f);
        }

        private void DropItems()
        {
            _OpenChestSoundHook.Play();
            if (EntityAsset is ChestAsset)
            {
                if (_dropList.Count == 0) return;
                while (_dropList.Count != 0)
                {
                    int i = Random.Range(0, _dropList.Count);
                    float roll = Random.Range(0.0f, 1.0f);
                    float dropChance = _dropList[i].GetDropChanceFromChest();
                    if (roll < dropChance && dropChance != 0)
                    {
                        int value = _dropList[i].GetItemValue();
                        if (_capacity >= value)
                        {
                            SubtractCapacity(value);
                            SetSpawnPoint();
                            _dropList[i].SpawnEntity(spawnPoint, _diContainer, _roomController.transform);
                            _isAnyItemDrop = true;
                        }
                    }
                    _dropList.RemoveAt(i);
                }
            }
            else
            {
                Debug.Log("Incorrect type of EntityAsset set, only ChestAsset allowed!");
                return;
            }
            if(_isAnyItemDrop == false)
            {
                _badlyItem.SpawnEntity(spawnPoint, _diContainer, _roomController.transform);
            }
        }
        private void SetDropList()
        {
            if (EntityAsset is ChestAsset)
            {
                var e = (EntityAsset as ChestAsset).DropList;
                _dropList = new List<Entity>(e);
                ShuffleDropList();

            }
        }
        private void SetSpawnPoint()
        {
            spawnPoint = _spawnLootArea.GetRandomPointInsideCircle();
        }
        private void SubtractCapacity(int value)
        {
            _capacity -= value;

        }
        private void ShuffleDropList()
        {
            var random = new System.Random();
            for (int i = _dropList.Count - 1; i >= 1; i--)
            {
                int j = random.Next(i + 1);
                var temp = _dropList[j];
                _dropList[j] = _dropList[i];
                _dropList[i] = temp;
            }
        }
        private void ChooseBadlyItem()
        {
            int index = int.MaxValue;
            for(int i = 0; i < _dropList.Count; i++ )
            {
                if (_dropList[i].ItemValue < index)
                {
                    index = _dropList[i].ItemValue;
                    _badlyItem = _dropList[i];
                }
            }
        }
        private void DestroyChest()
        {
            if (_isOpened)
            {
                Instantiate(_impactEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
