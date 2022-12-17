using Engine.UI.Canvas;
using Engine.UI.UiAttachSystem;
using Zenject;

namespace Engine.Installers
{
    public class CommonInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Ui();

        }


        private void Ui()
        {
            Container.BindInterfacesTo<CanvasContainer>().AsSingle();
            Container.BindInterfacesTo<UiAttachSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<HudWindowPresenter>().AsSingle();
        }
    }
}
