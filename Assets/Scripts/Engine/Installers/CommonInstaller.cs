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
            Container.BindInstance(_parentView);

            Container.BindInterfacesAndSelfTo<CardPropertiesDataProvider>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerHandController>().AsSingle().NonLazy();

            BindFactories();

            Container.BindInterfacesAndSelfTo<GameCircle>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<CardMoveController>().AsSingle().NonLazy();

            Signals();

            Ui();
        }

        private void Signals()
        {
            Container.BindInterfacesAndSelfTo<CardClickSignalHandler>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CardPointerSignalHandler>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CardDragSignalHandler>().AsSingle().NonLazy();
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