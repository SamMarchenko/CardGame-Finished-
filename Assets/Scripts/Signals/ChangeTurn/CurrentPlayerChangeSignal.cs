using Cards;

public class CurrentPlayerChangeSignal
{
    private readonly EPlayers _player;

    public CurrentPlayerChangeSignal(EPlayers player)
    {
        _player = player;
    }
}