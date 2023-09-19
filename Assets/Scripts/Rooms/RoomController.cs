using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace MysteryForest
{
    public class RoomController : MonoBehaviour
    {
        [SerializeField] private GameObject _viewPoint;
        [SerializeField] private int _roomDifficulty;

        [Inject] private RoomTransition _roomTransition;
        [Inject] private SoundPlayer _soundPlayer;
        [Inject] private DiContainer _diContainer;
        [Inject] private UICanvasManager _uICanvasManager;

        private BackGroundMusicController _backGroundMusicController;
        [SerializeField] private GameObject _owlStandPoint;
        public GameObject OwlStandPoint => _owlStandPoint;
        [SerializeField] private bool _isOwlRequired;
        [SerializeField] private bool _isSnakeSliderRequired = false;
        [SerializeField] private bool _isTutorial = false;

        public bool IsOwlRequired => _isOwlRequired;
        [SerializeField] private List<Chest> _chestPrefabs;
        private Gate[] _gates;
        private List<Destructible> _enemy;
        private bool _isActive = false;
        public bool IsActive => _isActive;
        public GameObject ViewPoint => _viewPoint;
        public event UnityAction OnRoomCompleted;
        [SerializeField] private UnityEvent _onAllEnemyInRoomDead;
        public UnityEvent OnAllEnemyInRoomDead => _onAllEnemyInRoomDead;
        [SerializeField] private UnityEvent _onClearRoomEnter;
        public UnityEvent OnClearRoomEnter => _onClearRoomEnter;
        public Action OnDangerRoomEnter;
        private EnemySpawner _enemySpawner;
        public Action OnPlayerLeaveRoom;
        private bool _isChestAllreadySpawn = false;
        
        

        private void Awake()
        {
            _gates = GetComponentsInChildren<Gate>();
            _enemy = new List<Destructible>(GetComponentsInChildren<Destructible>());
            _backGroundMusicController = _soundPlayer.GetComponentInChildren<BackGroundMusicController>();
            _enemySpawner = GetComponentInChildren<EnemySpawner>();
        }
        private void Start()
        {
            foreach (var gate in _gates)
            {
                gate.OnPlayerGateEnter += OnRoomPlayerEnter;
            }
            _enemySpawner.OnEnemySpawn += OnEnemySpawn;
            _enemySpawner.SpawnEnemies(_roomDifficulty);
            for (int i = 0; i < _enemy.Count; i++)
            {
                _enemy[i].OnDeath += OnEnemyDie;

                if (_enemy[i].GetComponent<Hive>() != null)
                    (_enemy[i].GetComponent<Hive>()).SpawnHornet += OnEnemySpawn;
            }
            SetDestructableSate(_isActive);

            CheckRoomForEnemy();
        }

        private void OnRoomPlayerEnter(Gate gate)
        {
            _roomTransition.SetDestinationRoom(gate);
        }

        private void OnEnemyDie(Destructible des)
        {
            _enemy.Remove(des);
            CheckRoomForEnemy();
        }

        private void CheckRoomForEnemy()
        {
            if (_enemy.Count == 0)
            {
                if (_isChestAllreadySpawn == false)
                {
                    SpawnChest();
                }
                OpenGates();
                OnAllEnemyInRoomDead?.Invoke();
                _backGroundMusicController.PlayFriendlyMusic();
            }
        }

        private void OnDestroy()
        {
            _enemySpawner.OnEnemySpawn -= OnEnemySpawn;
            for (int i = 0; i < _enemy.Count; i++)
            {
                _enemy[i].OnDeath -= OnEnemyDie;

                if (_enemy[i].GetComponent<Hive>() != null)
                    (_enemy[i].GetComponent<Hive>()).SpawnHornet -= OnEnemySpawn;
            }

            foreach (var gate in _gates)
            {
                gate.OnPlayerGateEnter -= OnRoomPlayerEnter;
            }
        }

        private void OnEnemySpawn(GameObject enemy)
        {
            Destructible des = enemy.GetComponent<Destructible>();
            des.OnDeath += OnEnemyDie;
            _enemy.Add(des);
        }

        public void Enter()
        {
            _isActive = true;
            SetDestructableSate(_isActive);

            if (_enemy.Count == 0) 
            {
                OnClearRoomEnter?.Invoke();
                _backGroundMusicController.PlayFriendlyMusic();
            }
            else 
            { 
                OnDangerRoomEnter?.Invoke();
                _backGroundMusicController.PlayBattleMusic();
            }

            if (_isSnakeSliderRequired)
            {
                if (_uICanvasManager.TryGetComponent<UIUpdateSnakeHealth>(out var uIUpdateSnakeHealth))
                    uIUpdateSnakeHealth.ShowSlider();

            }

            if (!_isTutorial)
            {
                if (_uICanvasManager.TryGetComponent<StatesLevel>(out var statesLevel))
                    statesLevel.FadeOutTutorialText();
            }

            else if (_uICanvasManager.TryGetComponent<StatesLevel>(out var statesLevel))
                statesLevel.ShowTutorialText();
        }
        public void Leave()
        {
            OnPlayerLeaveRoom?.Invoke();
            _isActive = false;
        }

        private void SetDestructableSate(bool state)
        {
            for (int i = 0; i < _enemy.Count; i++)
            {
                if (_enemy[i].TryGetComponent<AIControllerBase>(out var enemy))
                    enemy.AIEnable(state);
                //TODO временное решение, что бы убрать баг со звуком у шершня
                if (_enemy[i].TryGetComponent<HornetAI>(out var hornet))
                    hornet.GetComponent<Destructible>().enabled = state;

                if (_enemy[i].TryGetComponent<Hive>(out var hive))
                    hive.enabled = state;
            }
        }

        private void SpawnChest()
        {
            List<Chest> chests = _chestPrefabs;

            while (chests.Count > 0)
            {
                var randomIndex = UnityEngine.Random.Range(0, chests.Count);
                var chanceProc = UnityEngine.Random.Range(0f, 1f);

                //print(chests[randomIndex] + " " + chests[randomIndex].SpawnChance + " " + chanceProc + " " + chests.Count);

                if (chanceProc > chests[randomIndex].SpawnChance)
                {
                    chests.RemoveAt(randomIndex);
                    continue;
                }
                else
                {
                    chests[randomIndex].SpawnEntity(transform.position, _diContainer, transform);
                    break;
                }
            }
            _isChestAllreadySpawn = true;
        }

        private void OpenGates()
        {
            for (int i = 0; i < _gates.Length; i++)
            {
                _gates[i].OpenGate();
            }
            OnRoomCompleted?.Invoke();
        }

    }
}
