using System;
using Cards;
using TMPro;
using Zenject;

namespace DefaultNamespace
{
    public class PlayersProvider : IInitializable, IChangeStageListener, IChangeCurrentPlayerListener
    {
        private Player _firstPlayer;
        private Player _secondPlayer;
        private PlayerFactory _playerFactory;
        private readonly DeckBuilder _deckBuilder;

        public PlayersProvider(PlayerFactory playerFactory, DeckBuilder deckBuilder)
        {
            _playerFactory = playerFactory;
            _deckBuilder = deckBuilder;
        }

        public void Initialize()
        {
            CreatePlayers();
            GiveDeckToPlayers();
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

        private void GiveDeckToPlayers()
        {
            _firstPlayer.SetDeck(_deckBuilder.GetFullDeck(EPlayers.FirstPlayer));
            _secondPlayer.SetDeck(_deckBuilder.GetFullDeck(EPlayers.SecondPlayer));
        }

        public void OnStageChange(StageChangeSignal signal)
        {
            throw new NotImplementedException();
        }

        public void OnCurrentPlayerChange(CurrentPlayerChangeSignal signal)
        {
            throw new NotImplementedException();
        }
    }
}