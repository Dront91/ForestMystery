using TowerDefense;
using UnityEngine;
using Zenject;
using System;
namespace MysteryForest
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private CircleArea[] _spawnPoints;
        [SerializeField] private GameObject[] _enemyPrefabs;
        [Inject] private DiContainer _diContainer;
        private RoomController _roomController;
        public Action<GameObject> OnEnemySpawn;

        private void Awake()
        {
            _roomController = GetComponentInParent<RoomController>();
        }
        public void SpawnEnemies(int roomDifficulty)
        {
            while (roomDifficulty > 0)
            {
                var enemy = ChooseRandomEnemy();
                if(enemy.GetComponent<AIControllerBase>().EnemyStrength > roomDifficulty)
                    continue;
                
                var spawnedEnemy = _diContainer.InstantiatePrefab(enemy, _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Length)].GetRandomPointInsideCircle(), 
                    Quaternion.identity, _roomController.transform);
                OnEnemySpawn?.Invoke(spawnedEnemy);
                roomDifficulty -= spawnedEnemy.GetComponent<AIControllerBase>().EnemyStrength;
            }
        }
        private GameObject ChooseRandomEnemy()
        {
            return _enemyPrefabs[UnityEngine.Random.Range(0, _enemyPrefabs.Length)];
        }
       
    }
}
