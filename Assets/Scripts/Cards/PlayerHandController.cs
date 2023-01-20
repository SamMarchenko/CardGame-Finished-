using Cards;
using Zenject;

namespace DefaultNamespace
{
    public class PlayerHandController : IInitializable
    {
        private readonly PlayerHandFactory _playerHandFactory;
        private PlayerHandView _player1HandView;
        private PlayerHandView _player2HandView;

        public PlayerHandController(PlayerHandFactory playerHandFactory)
        {
            _playerHandFactory = playerHandFactory;
        }

        public void Initialize()
        {
            _player1HandView = _playerHandFactory.CreatePlayerHand(EPlayers.FirstPlayer);
            _player2HandView = _playerHandFactory.CreatePlayerHand(EPlayers.SecondPlayer);
        }

        public void SetNewCard(EPlayers currentPlayer,CardView cardView)
        {
            if (currentPlayer == EPlayers.FirstPlayer)
            {
                _player1HandView.SetNewCard(cardView);
            }
            else
            {
                _player2HandView.SetNewCard(cardView);
            }
        }
    }
}