using System.Collections.Generic;

public class ChangeCurrentPlayerSignalHandler
{
    private readonly List<IChangeCurrentPlayerListener> _listeners;

    public ChangeCurrentPlayerSignalHandler(List<IChangeCurrentPlayerListener> listeners)
    {
        _listeners = listeners;
    }

    public void Fire(CurrentPlayerChangeSignal signal)
    {
        foreach (var listener in _listeners)
        {
            listener.OnCurrentPlayerChange(signal);
        }
    }
}