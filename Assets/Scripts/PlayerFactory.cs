using Cards;

namespace DefaultNamespace
{
    public class PlayerFactory
    {
        private readonly ParentView _parentView;

        public PlayerFactory(ParentView parentView)
        {
            _parentView = parentView;
        }

        public Player CreatePlayer(EPlayers playerType)
        {
            var player = new Player();
            player.Init(_parentView, playerType);
            
            return player;
        }
    }
}