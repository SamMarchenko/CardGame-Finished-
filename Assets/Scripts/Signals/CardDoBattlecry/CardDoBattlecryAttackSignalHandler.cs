using System.Collections.Generic;

public class CardDoBattlecryAttackSignalHandler
{
    private readonly List<ICardBattlecryAttackListener> _listeners;

    public CardDoBattlecryAttackSignalHandler(List<ICardBattlecryAttackListener> listeners)
    {   
        _listeners = listeners;
    }

    public void OnBattlecryAttackPlayerFire(CardBattlecryAttackSignal signal)
    {
        foreach (var listener in _listeners)
        {
            listener.OnDoBattlecryAttack(signal);
        }
    }
}