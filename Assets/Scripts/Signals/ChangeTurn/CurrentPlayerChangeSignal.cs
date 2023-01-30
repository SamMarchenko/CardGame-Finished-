using Cards;
using DefaultNamespace;

public class CurrentPlayerChangeSignal
{
    public readonly EPlayers PlayerType;
    public readonly Player Player;

    public CurrentPlayerChangeSignal(EPlayers playerType, Player player)
    {
        PlayerType = playerType;
        Player = player;
    }
}