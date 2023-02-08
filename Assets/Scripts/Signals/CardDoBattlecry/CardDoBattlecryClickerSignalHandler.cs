using System.Collections.Generic;

public class CardDoBattlecryClickerSignalHandler
{
    private readonly List<ICardBattlecryClickerListener> _listeners;

    public CardDoBattlecryClickerSignalHandler(List<ICardBattlecryClickerListener> listeners)
    {   
        _listeners = listeners;
    }

    public void OnBattlecryClickerFire(CardBattlecryClickerSignal signal)
    {
        foreach (var listener in _listeners)
        {
            listener.OnDoBattlecryClick(signal);
        }
    }
}