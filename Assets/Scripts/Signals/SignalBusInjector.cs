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
        private readonly CardSignalBus _bus;
        private readonly CardClickSignalHandler _cardClickSignalHandler;
        private readonly CardPointerSignalHandler _cardPointerSignalHandler;
        private readonly CardDragSignalHandler _cardDragSignalHandler;
        private readonly ChangeStageSignalHandler _changeStageSignalHandler;
        private readonly ChangeCurrentPlayerSignalHandler _changeCurrentPlayerSignalHandler;

        public SignalBusInjector(CardSignalBus bus,
            CardClickSignalHandler cardClickSignalHandler,
            CardPointerSignalHandler cardPointerSignalHandler,
            CardDragSignalHandler cardDragSignalHandler,
            ChangeStageSignalHandler changeStageSignalHandler,
            ChangeCurrentPlayerSignalHandler changeCurrentPlayerSignalHandler)
        {
            _bus = bus;
            _cardClickSignalHandler = cardClickSignalHandler;
            _cardPointerSignalHandler = cardPointerSignalHandler;
            _cardDragSignalHandler = cardDragSignalHandler;
            _changeStageSignalHandler = changeStageSignalHandler;
            _changeCurrentPlayerSignalHandler = changeCurrentPlayerSignalHandler;
        }

        // комментарии потом можешь почистить,
        // внедряем зависимости не через конструктор, чтобы избежать циклической зависимости
        public void Initialize()
        {
            _bus.Init(_cardClickSignalHandler,
                _cardPointerSignalHandler,
                _cardDragSignalHandler,
                _changeStageSignalHandler,
                _changeCurrentPlayerSignalHandler);
        }
    }
}