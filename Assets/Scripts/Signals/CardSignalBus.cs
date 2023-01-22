namespace Signals
{
    public class CardSignalBus
    {
        private  CardClickSignalHandler _cardClickSignalHandler;
        private  CardPointerSignalHandler _cardPointerSignalHandler;
        private  CardDragSignalHandler _cardDragSignalHandler;
        private  ChangeStageSignalHandler _changeStageSignalHandler;
        private  ChangeCurrentPlayerSignalHandler _changeCurrentPlayerSignalHandler;
        
        
        public void Init(
            CardClickSignalHandler cardClickSignalHandler, 
            CardPointerSignalHandler cardPointerSignalHandler, 
            CardDragSignalHandler cardDragSignalHandler, 
            ChangeStageSignalHandler changeStageSignalHandler,
            ChangeCurrentPlayerSignalHandler changeCurrentPlayerSignalHandler)
        {
            _cardClickSignalHandler = cardClickSignalHandler;
            _cardPointerSignalHandler = cardPointerSignalHandler;
            _cardDragSignalHandler = cardDragSignalHandler;
            _changeStageSignalHandler = changeStageSignalHandler;
            _changeCurrentPlayerSignalHandler = changeCurrentPlayerSignalHandler;
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
        
        public void StageChangeFire(StageChangeSignal signal)
        {
            _changeStageSignalHandler.Fire(signal);
        }
        
        public void CurrentPlayerChangeFire(CurrentPlayerChangeSignal signal)
        {
            _changeCurrentPlayerSignalHandler.Fire(signal);
        }
    }
}