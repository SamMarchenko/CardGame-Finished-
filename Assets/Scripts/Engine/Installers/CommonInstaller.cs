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
        [SerializeField] private PlayerHandView _playerHandView;
        public override void InstallBindings()
        {
            Container.BindInstance(_parentView);
            Container.BindInstance(_playerHandView);
            Container.BindInterfacesAndSelfTo<CardPropertiesDataProvider>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerHandController>().AsSingle().NonLazy();
            
            BindFactories();
            
            Container.BindInterfacesAndSelfTo<GameCircle>().AsSingle().NonLazy();
            
            
            Ui();
        }


        private void Ui()
        {
            Container.BindInterfacesTo<CanvasContainer>().AsSingle();
            Container.BindInterfacesTo<UiAttachSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<HudWindowPresenter>().AsSingle();
        }

        private void BindFactories()
        {
            Container.BindInterfacesAndSelfTo<DeckFactory>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerHandFactory>().AsSingle().NonLazy();
        }
    }
}
