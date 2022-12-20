using DefaultNamespace;
using Engine.UI.Canvas;
using Engine.UI.UiAttachSystem;
using UnityEngine;
using Zenject;

namespace Engine.Installers
{
    public class CommonInstaller : MonoInstaller
    {
        [SerializeField] private ParentView _parentView;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CardPropertiesDataProvider>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<DeckFactory>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameCircle>().AsSingle().NonLazy();
            
            
            Container.BindInstance(_parentView);
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
