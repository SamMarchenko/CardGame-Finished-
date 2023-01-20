using System;
using Cards;
using TMPro;
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

        public Player GetPlayer(EPlayers player)
        {
            return player == EPlayers.FirstPlayer ? _firstPlayer : _secondPlayer;
        }
        
        private void CreatePlayers()
        {
            _firstPlayer = _playerFactory.CreatePlayer(EPlayers.FirstPlayer);
            _secondPlayer = _playerFactory.CreatePlayer(EPlayers.SecondPlayer);
        }

    }
}