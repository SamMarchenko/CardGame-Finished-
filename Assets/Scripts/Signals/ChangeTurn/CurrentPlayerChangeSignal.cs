using Cards;

public class CurrentPlayerChangeSignal
{
    public readonly EPlayers Player;

    public CurrentPlayerChangeSignal(EPlayers player)
    {
        Player = player;
    }
}