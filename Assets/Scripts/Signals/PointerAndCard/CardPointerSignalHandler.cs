using System.Collections.Generic;

public class CardPointerSignalHandler
{
    private readonly List<ICardPointerListener> _listeners;

    public CardPointerSignalHandler(List<ICardPointerListener>  listeners)
    {
        _listeners = listeners;
    }

    public void FirePointerOn(CardPointerSignal signal)
    {
        foreach (var listener in _listeners)
        {
            listener.PointerOnCard(signal);
        }
    }
    
    public void FirePointerOff(CardPointerSignal signal)
    {
        foreach (var listener in _listeners)
        {
            listener.PointerOffCard(signal);
        }
    }
}