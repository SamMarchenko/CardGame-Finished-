using System.Collections.Generic;

public class CardDragSignalHandler
{
    private readonly List<ICardDragListener> _listeners;

    public CardDragSignalHandler(List<ICardDragListener>  listeners)
    {
        _listeners = listeners;
    }

    public void FireDragging(CardDragSignal signal)
    {
        foreach (var listener in _listeners)
        {
            listener.OnDraggingCard(signal);
        }
    }
    
    public void FireDragEnd(CardDragSignal signal)
    {
        foreach (var listener in _listeners)
        {
            listener.OnDragCardEnd(signal);
        }
    }
}