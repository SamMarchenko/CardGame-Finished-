public interface ICardDragListener
{
    void OnDragCardStart(CardDragSignal signal);
    void OnDraggingCard(CardDragSignal signal);
    void OnDragCardEnd(CardDragSignal signal);
}