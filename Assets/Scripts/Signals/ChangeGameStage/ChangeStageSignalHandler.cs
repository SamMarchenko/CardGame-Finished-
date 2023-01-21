using System.Collections.Generic;

public class ChangeStageSignalHandler
{
    private readonly List<IChangeStageListener> _listeners;

    public ChangeStageSignalHandler(List<IChangeStageListener> listeners)
    {
        _listeners = listeners;
    }

    public void Fire(StageChangeSignal signal)
    {
        foreach (var listener in _listeners)
        {
            listener.OnStageChange(signal);
        }
    }
}