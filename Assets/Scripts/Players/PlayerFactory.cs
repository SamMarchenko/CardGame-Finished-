using Cards;
using Signals;

namespace DefaultNamespace
{
    public class PlayerFactory
    {
        private readonly ParentView _parentView;
        private readonly PlayerSignalBus _playerSignalBus;
        private readonly AbilitiesProvider _abilitiesProvider;
        private readonly BuffController _buffController;
        private readonly PlayerView _firstPlayerView;
        private readonly PlayerView _secondPlayerView;

        public PlayerFactory(ParentView parentView, AllPlayersView allPlayersView, PlayerSignalBus playerSignalBus,
        AbilitiesProvider abilitiesProvider, BuffController buffController)
        {
            _parentView = parentView;
            _playerSignalBus = playerSignalBus;
            _abilitiesProvider = abilitiesProvider;
            _buffController = buffController;
            _firstPlayerView = allPlayersView.FirstPlayerView;
            _secondPlayerView = allPlayersView.SecondPlayerView;
        }

        public Player CreatePlayer(EPlayers playerType)
        {
            var playerView = playerType == EPlayers.FirstPlayer ? _firstPlayerView : _secondPlayerView;
            var player = new Player();
            player.Init(_parentView, playerType, playerView, _playerSignalBus, _abilitiesProvider, _buffController);

            return player;
        }
    }
}