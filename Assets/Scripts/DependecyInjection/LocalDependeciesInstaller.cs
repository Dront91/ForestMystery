using UnityEngine;
using Zenject;

namespace MysteryForest
{   
    public class LocalDependeciesInstaller : MonoInstaller
    {
        [SerializeField] private RoomTransition roomTransition;
        [SerializeField] private AIOwl _aiOwl;
        

        public override void InstallBindings()
        {
            Container.Bind<RoomTransition>().FromInstance(roomTransition).AsSingle();
            Container.Bind<AIOwl>().FromComponentInNewPrefab(_aiOwl).AsSingle();
        }
    }
}
