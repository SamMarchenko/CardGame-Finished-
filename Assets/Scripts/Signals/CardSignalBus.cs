namespace Signals
{
    public class CardSignalBus
    {
        private CardClickSignalHandler _cardClickSignalHandler;
        private CardPointerSignalHandler _cardPointerSignalHandler;
        private CardDragSignalHandler _cardDragSignalHandler;
        private CardDoBattlecrySignalHandler _cardDoBattlecrySignalHandler;
        private CardDoBattlecryClickerSignalHandler _cardDoBattlecryClickerSignalHandler;


        public void Init(
            CardClickSignalHandler cardClickSignalHandler,
            CardPointerSignalHandler cardPointerSignalHandler,
            CardDragSignalHandler cardDragSignalHandler,
            CardDoBattlecrySignalHandler cardDoBattlecrySignalHandler,
            CardDoBattlecryClickerSignalHandler cardDoBattlecryClickerSignalHandler)
        {
            _cardClickSignalHandler = cardClickSignalHandler;
            _cardPointerSignalHandler = cardPointerSignalHandler;
            _cardDragSignalHandler = cardDragSignalHandler;
            _cardDoBattlecrySignalHandler = cardDoBattlecrySignalHandler;
            _cardDoBattlecryClickerSignalHandler = cardDoBattlecryClickerSignalHandler;
        }

        public void CardDoBattlecryFire(CardDoBattlecrySignal signal)
        {
            _cardDoBattlecrySignalHandler.OnStartBattlecryFire(signal);
        }

        public void CardBattlecryClicker(CardBattlecryClickerSignal signal)
        {
            _cardDoBattlecryClickerSignalHandler.OnBattlecryClickerFire(signal);
        }

        public void CardClickFire(CardClickSignal signal)
        {
            _cardClickSignalHandler.Fire(signal);
        }

        public void FirePointerOn(CardPointerSignal signal)
        {
            _cardPointerSignalHandler.FirePointerOn(signal);
        }

        public void FirePointerOff(CardPointerSignal signal)
        {
            _cardPointerSignalHandler.FirePointerOff(signal);
        }

        public void FireDragStart(CardDragSignal signal)
        {
            _cardDragSignalHandler.FireDragStart(signal);
        }

        public void FireDragging(CardDragSignal signal)
        {
            _cardDragSignalHandler.FireDragging(signal);
        }

        public void FireDragEnd(CardDragSignal signal)
        {
            _cardDragSignalHandler.FireDragEnd(signal);
        }
    }
}