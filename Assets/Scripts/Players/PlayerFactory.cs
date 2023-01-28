using Cards;

namespace DefaultNamespace
{
    public class PlayerFactory
    {
        private readonly ParentView _parentView;
        private readonly PlayerView _firstPlayerView;
        private readonly PlayerView _secondPlayerView;

        public PlayerFactory(ParentView parentView, AllPlayersView allPlayersView)
        {
            _parentView = parentView;
            _firstPlayerView = allPlayersView.FirstPlayerView;
            _secondPlayerView = allPlayersView.SecondPlayerView;
        }

        public Player CreatePlayer(EPlayers playerType)
        {
            var playerView = playerType == EPlayers.FirstPlayer ? _firstPlayerView : _secondPlayerView;
            var player = new Player();
            player.Init(_parentView, playerType, playerView);

            return player;
        }
    }
}