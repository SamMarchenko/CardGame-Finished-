using Cards;
using Zenject;

namespace DefaultNamespace
{
    public class PlayersProvider : IInitializable
    {
        private Player _firstPlayer;
        private Player _secondPlayer;
        private PlayerFactory _playerFactory;

        public PlayersProvider(PlayerFactory playerFactory)
        {
            _playerFactory = playerFactory;
        }

        public void Initialize()
        {
            CreatePlayers();
        }
        
        private void CreatePlayers()
        {
            _firstPlayer = _playerFactory.CreatePlayer(EPlayers.FirstPlayer);
            _secondPlayer = _playerFactory.CreatePlayer(EPlayers.SecondPlayer);
        }
    }
}