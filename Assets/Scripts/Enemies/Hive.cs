using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace MysteryForest
{
    public class Hive : AIControllerBase
    {
        
        [SerializeField] private GameObject _spawnPrefab;
        [SerializeField] private float _timeToSpawn;
        [SerializeField] private int _spawnAmount;
        [SerializeField] private PatrolPoint[] _patrolPoints;
        [Inject] private DiContainer _diContainer;

        public event UnityAction<GameObject> SpawnHornet;
        private RoomController _roomController;
        private Animator _animator;
        private float _spawnTime;
        private bool _isSpawning;
        private int _spawnCount;

        private const float _spawnAnimationTime = 0.6f;

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            _spawnTime += _timeToSpawn;
            _roomController = GetComponentInParent<RoomController>();
        }

        private void Update()
        {
            if (_spawnCount < _spawnAmount)
            {
                if (Time.time > _spawnTime - _spawnAnimationTime && _isSpawning == false)
                {
                    _animator.SetTrigger("spawn");

                    _isSpawning = true;
                }

                if (Time.time > _spawnTime)
                {
                    var hornet = _diContainer.InstantiatePrefab(_spawnPrefab, transform.position + Vector3.down, Quaternion.identity, _roomController.transform);
                   // HornetAI hornet = Instantiate(_spawnPrefab, transform.position + Vector3.down, Quaternion.identity).GetComponent<HornetAI>();
                    SpawnHornet?.Invoke(hornet);
                    hornet.GetComponent<HornetAI>().SetRoute(_patrolPoints);
                     _spawnTime = Time.time + _timeToSpawn;
                    _isSpawning = false;
                    _spawnCount++;
                }
            }          
        }
    }
}