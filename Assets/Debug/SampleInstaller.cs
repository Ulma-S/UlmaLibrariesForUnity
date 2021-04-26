using UnityEngine;
using Zenject;

public class SampleInstaller : MonoInstaller
{
    public override void InstallBindings() {
        Container.Bind<IInputProvider>()
            .To<KeyboardInputProvider>()
            .AsSingle();
    }
}