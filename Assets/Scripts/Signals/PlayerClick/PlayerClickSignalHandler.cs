using System.Collections.Generic;

public class PlayerClickSignalHandler
{
    private readonly List<IPlayerClickListener> _listeners;

    public PlayerClickSignalHandler(List<IPlayerClickListener> listeners)
    {
        _listeners = listeners;
    }

    public void Fire(PlayerClickSignal signal)
    {
        foreach (var listener in _listeners)
        {
            listener.OnPlayerClick(signal);
        }
    }
}