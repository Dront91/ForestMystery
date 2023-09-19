using UnityEngine;
using Zenject;


namespace MysteryForest
{
    public class UIManagerInstaller : MonoInstaller
    {
        [SerializeField] private UICanvasManager canvasManager;
    public override void InstallBindings()
        {
            Container.Bind<UICanvasManager>().FromInstance(canvasManager).AsSingle();
            Container.QueueForInject(canvasManager);
        }
    }
}
