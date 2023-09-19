using System.Collections;
using System.Collections.Generic;
using TowerDefense;
using UnityEngine;
using Zenject;
namespace MysteryForest
{
    public class EnemyInventory : MonoBehaviour
    {
        [SerializeField] private EnemyInventoryAsset _entityAsset;
        [SerializeField] private Enemy _enemy;
        [SerializeField] private CircleArea _spawnLootArea;
        [Inject] private DiContainer _diContainer;
        private List<Entity> _dropList;
        private Destructible _dest;
        public enum Enemy
        {
            Squirrel,
            Mole,
            Hedgehog,
            Hornet,
            Hive,
            Rabbit,
            SnakeBoss
        }
        public void Start()
        {
            _dest = GetComponent<Destructible>();
            _dest.OnDeath += DropItems;
            SetDropList();
        }
        private void OnDestroy()
        {
            _dest.OnDeath -= DropItems;
        }
        public void DropItems(Destructible d)
        {
            if (_dropList.Count == 0) return;
            while (_dropList.Count != 0)
            {
                int i = Random.Range(0, _dropList.Count);
                float roll = Random.Range(0.0f, 1.0f);
                float dropChance = _dropList[i].GetDropChanceFromEnemy(_enemy);
                if (roll < dropChance && dropChance != 0)
                {
                    _dropList[i].SpawnEntity(_spawnLootArea.GetRandomPointInsideCircle(), _diContainer, null);
                }
                _dropList.RemoveAt(i);
            }
        }
        private void SetDropList()
        {
                var e = _entityAsset.DropList;
                _dropList = new List<Entity>(e);
                if (_dropList.Count != 0)
                {
                    ShuffleDropList();
                }
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
    }
}
