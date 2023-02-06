using Zenject;

namespace Signals
{
    /// <summary>
    /// Нужен для того чтобы все обработчики сигналов передать в автобус, не единственное решение и можно было обойтись
    /// тем чтобы добавлять в автобус прям из обработчика но подумал что не самое крутое решение когда 2 класса друг о
    /// друге знают
    /// </summary>
    public class SignalBusInjector : IInitializable
    {
        private readonly CardSignalBus _cardSignalBus;
        private readonly PlayerSignalBus _playerSignalBus;
        private readonly CardClickSignalHandler _cardClickSignalHandler;
        private readonly CardPointerSignalHandler _cardPointerSignalHandler;
        private readonly CardDragSignalHandler _cardDragSignalHandler;
        private readonly CardDoBattlecrySignalHandler _cardDoBattlecrySignalHandler;
        private readonly ChangeStageSignalHandler _changeStageSignalHandler;
        private readonly ChangeCurrentPlayerSignalHandler _changeCurrentPlayerSignalHandler;
        private readonly PlayerClickSignalHandler _playerClickSignalHandler;

        public SignalBusInjector(CardSignalBus cardSignalBus,
            CardClickSignalHandler cardClickSignalHandler,
            CardPointerSignalHandler cardPointerSignalHandler,
            CardDragSignalHandler cardDragSignalHandler,
            CardDoBattlecrySignalHandler cardDoBattlecrySignalHandler,
            PlayerSignalBus playerSignalBus,
            ChangeStageSignalHandler changeStageSignalHandler,
            ChangeCurrentPlayerSignalHandler changeCurrentPlayerSignalHandler,
            PlayerClickSignalHandler playerClickSignalHandler)
        {
            _cardSignalBus = cardSignalBus;
            _cardClickSignalHandler = cardClickSignalHandler;
            _cardPointerSignalHandler = cardPointerSignalHandler;
            _cardDragSignalHandler = cardDragSignalHandler;
            _cardDoBattlecrySignalHandler = cardDoBattlecrySignalHandler;

            _playerSignalBus = playerSignalBus;
            _changeStageSignalHandler = changeStageSignalHandler;
            _changeCurrentPlayerSignalHandler = changeCurrentPlayerSignalHandler;
            _playerClickSignalHandler = playerClickSignalHandler;
        }
        
        // внедряем зависимости не через конструктор, чтобы избежать циклической зависимости
        public void Initialize()
        {
            _cardSignalBus.Init(_cardClickSignalHandler,
                _cardPointerSignalHandler,
                _cardDragSignalHandler, _cardDoBattlecrySignalHandler);

            _playerSignalBus.Init(_changeStageSignalHandler,
                _changeCurrentPlayerSignalHandler,
                _playerClickSignalHandler);
        }
    }
}