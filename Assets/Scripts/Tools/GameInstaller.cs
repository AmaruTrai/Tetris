using Game;
using Zenject;

namespace Tools
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BlockPool>().FromComponentInHierarchy().AsSingle();
            Container.Bind<Block>().FromComponentsInHierarchy().AsTransient();
            Container.Bind<BaseSpace>().FromComponentInHierarchy().AsSingle();
        }
    }
}

