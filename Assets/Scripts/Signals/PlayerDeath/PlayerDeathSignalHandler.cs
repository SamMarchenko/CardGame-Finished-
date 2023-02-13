using System.Collections.Generic;

public class PlayerDeathSignalHandler
{
    private readonly List<IPlayerDeathListener> _listeners;

    public PlayerDeathSignalHandler(List<IPlayerDeathListener> listeners)
    {
        _listeners = listeners;
    }

    public void Fire(PlayerDeathSignal signal)
    {
        foreach (var listener in _listeners)
        {
            listener.OnPlayerDeath(signal);
        }
    }
}