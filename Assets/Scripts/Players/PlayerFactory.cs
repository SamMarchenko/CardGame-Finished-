using Cards;
using Signals;

namespace DefaultNamespace
{
    public class PlayerFactory
    {
        private readonly ParentView _parentView;
        private readonly PlayerSignalBus _playerSignalBus;
        private readonly AbilitiesProvider _abilitiesProvider;
        private readonly PlayerView _firstPlayerView;
        private readonly PlayerView _secondPlayerView;

        public PlayerFactory(ParentView parentView, AllPlayersView allPlayersView, PlayerSignalBus playerSignalBus,
        AbilitiesProvider abilitiesProvider)
        {
            _parentView = parentView;
            _playerSignalBus = playerSignalBus;
            _abilitiesProvider = abilitiesProvider;
            _firstPlayerView = allPlayersView.FirstPlayerView;
            _secondPlayerView = allPlayersView.SecondPlayerView;
        }

        public Player CreatePlayer(EPlayers playerType)
        {
            var playerView = playerType == EPlayers.FirstPlayer ? _firstPlayerView : _secondPlayerView;
            var player = new Player();
            player.Init(_parentView, playerType, playerView, _playerSignalBus, _abilitiesProvider);

            return player;
        }
    }
}