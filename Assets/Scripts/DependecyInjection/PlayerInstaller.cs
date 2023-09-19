using UnityEngine;
using Zenject;

namespace MysteryForest
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private Transform _startPoint;
        [SerializeField] private GameObject _playerPrefab;

        public override void InstallBindings()
        {
            PlayerController playerController =
                Container.InstantiatePrefabForComponent<PlayerController>(
                    _playerPrefab, _startPoint.position, Quaternion.identity, null);

            Container.Bind<PlayerController>().FromInstance(playerController).AsSingle();

        }
    }
}
