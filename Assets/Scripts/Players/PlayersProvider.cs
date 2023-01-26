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
        private Player _currentPlayer;
        private PlayerFactory _playerFactory;
        private readonly DeckBuilder _deckBuilder;
        private ECurrentStageType _stageType;
        private EPlayers _currentPlayerType;

        public PlayersProvider(PlayerFactory playerFactory, DeckBuilder deckBuilder)
        {
            _playerFactory = playerFactory;
            _deckBuilder = deckBuilder;
        }

        public void Initialize()
        {
            CreatePlayers();
            GiveDeckBuilderToPlayers();
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

        private void GiveDeckBuilderToPlayers()
        {
            _firstPlayer.SetDeckBuilder(_deckBuilder);
            _secondPlayer.SetDeckBuilder(_deckBuilder);
        }

        public void OnStageChange(StageChangeSignal signal)
        {
            _stageType = signal.StageType;
            _firstPlayer.SetCurrentStageType(_stageType);
            _secondPlayer.SetCurrentStageType(_stageType);
        }

        public void OnCurrentPlayerChange(CurrentPlayerChangeSignal signal)
        {
            _currentPlayerType = signal.Player;
            _currentPlayer = GetPlayer(_currentPlayerType);
            RollCardsInHand();
            
            switch (_stageType)
            {
                case ECurrentStageType.MoveStage:
                    _currentPlayer.StartOfMove();
                    break;
             
            }
        }

        private void RollCardsInHand()
        {
            if (_currentPlayer == _firstPlayer)
            {
                _currentPlayer.RollCardsInHand(true);
                _secondPlayer.RollCardsInHand(false);
            }
            else
            {
                _currentPlayer.RollCardsInHand(true);
                _firstPlayer.RollCardsInHand(false);
            }
        }
    }
}