using System.Collections.Generic;

public class CardDoBattlecrySignalHandler
{
    private readonly List<ICardDoBattlecryListener> _listeners;

    public CardDoBattlecrySignalHandler(List<ICardDoBattlecryListener> listeners)
    {
        _listeners = listeners;
    }

    public void Fire(CardDoBattlecrySignal signal)
    {
        foreach (var listener in _listeners)
        {
            listener.OnCardDoBattlecry(signal);
        }
    }
}