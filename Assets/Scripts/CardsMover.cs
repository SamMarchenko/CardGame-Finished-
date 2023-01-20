using Cards;
using Zenject;

namespace DefaultNamespace
{
    public class CardsMover : IInitializable
    {
        private readonly PlayerFactory _playerFactory;
        private Player _player1;
        private Player _player2;

        public CardsMover(PlayerFactory playerFactory)
        {
            _playerFactory = playerFactory;
        }

        public void Initialize()
        {
            _player1 = _playerFactory.CreatePlayer(EPlayers.FirstPlayer);
            
        }
    }
}