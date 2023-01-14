using System.Collections.Generic;

public class CardClickSignalHandler
{
    private readonly List<ICardClickListener> _listeners;

    public CardClickSignalHandler(List<ICardClickListener>  listeners)
    {
        _listeners = listeners;
    }

    public void Fire(CardClickSignal signal)
    {
        foreach (var listener in _listeners)
        {
            listener.OnCardClick(signal);
        }
    }
}