using System;
using UnityEngine;
using Zenject;

namespace MysteryForest
{
    public class GlobalDependencyInstaller : MonoInstaller
    {
        [SerializeField] private LevelSequenceController levelSequenceController;
        [SerializeField] private AccountDataSaver _accountDataSaver;
        [SerializeField] private UICanvasManager canvasManager;
        [SerializeField] private GameObject _soundPlayerPrefab;
        [SerializeField] private SoundPlayer _soundPlayer;

        public override void InstallBindings()
        {

            Container.Bind<LevelSequenceController>().FromComponentInNewPrefab(levelSequenceController).AsSingle();
            Container.Bind<AccountDataSaver>().FromComponentInNewPrefab(_accountDataSaver).AsSingle();
            Container.Bind<UICanvasManager>().FromComponentInNewPrefab(canvasManager).AsSingle();

            // SoundPlayer SoundPlayer =
            //    Container.InstantiatePrefabForComponent<SoundPlayer>(_soundPlayerPrefab, transform.position, Quaternion.identity, null);
            // Container.Bind<SoundPlayer>().FromInstance(SoundPlayer).AsSingle();
            Container.Bind<SoundPlayer>().FromComponentInNewPrefab(_soundPlayer).AsSingle();
           
        }
    }
}
