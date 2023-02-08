using DefaultNamespace;
using Engine.UI.Canvas;
using Engine.UI.UiAttachSystem;
using Signals;
using UnityEngine;
using Zenject;

namespace Engine.Installers
{
    public class CommonInstaller : MonoInstaller
    {
        [SerializeField] private ParentView _parentView;
        [SerializeField] private AllPlayersView _allPlayersView;

        public override void InstallBindings()
        {
            Container.BindInstance(_parentView);
            Container.BindInstance(_allPlayersView);

            Container.BindInterfacesAndSelfTo<CardPropertiesDataProvider>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<Abilities>().AsSingle().NonLazy();
            
            BindFactories();

            // Порядок важен
            BindSignals();
            Container.BindInterfacesAndSelfTo<PlayersProvider>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<DeckBuilder>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameCircle>().AsSingle().NonLazy();
            

            Container.BindInterfacesAndSelfTo<CardMoverView>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<DamageController>().AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<AbilitiesProvider>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BuffController>().AsSingle().NonLazy();
            

            

            Ui();
        }

        private void BindSignals()
        {
            Container.BindInterfacesAndSelfTo<CardClickSignalHandler>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CardPointerSignalHandler>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CardDragSignalHandler>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CardDoBattlecrySignalHandler>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CardDoBattlecryClickerSignalHandler>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ChangeStageSignalHandler>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ChangeCurrentPlayerSignalHandler>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerClickSignalHandler>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SignalBusInjector>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CardSignalBus>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerSignalBus>().AsSingle().NonLazy();
        }

        private void Ui()
        {
            Container.BindInterfacesTo<CanvasContainer>().AsSingle();
            Container.BindInterfacesTo<UiAttachSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<HudWindowPresenter>().AsSingle();
        }

        private void BindFactories()
        {
            Container.BindInterfacesAndSelfTo<PlayerFactory>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CardFactory>().AsSingle().NonLazy();
        }
    }
}