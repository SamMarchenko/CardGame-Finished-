public interface ICardDragListener
{
    void OnDraggingCard(CardDragSignal signal);
    void OnDragCardEnd(CardDragSignal signal);
}