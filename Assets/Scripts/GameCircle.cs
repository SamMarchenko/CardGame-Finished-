using System.Collections.Generic;
using Cards;
using DefaultNamespace;
using UnityEngine;
using Zenject;

public class GameCircle : IInitializable, ITickable
{
    private ECurrentStageType _currentStage;
    private EPlayers _currentPlayerType = EPlayers.FirstPlayer;
    private readonly DeckBuilder _deckBuilder;
    private readonly CardMoverView _cardMoverView;
    private readonly PlayersProvider _playersProvider;
    private List<CardView> _currentDeck;
    private Player _currentPlayer;
    
    

    public GameCircle(DeckBuilder deckBuilder, CardMoverView cardMoverView, PlayersProvider playersProvider)
    {
        _deckBuilder = deckBuilder;
        _cardMoverView = cardMoverView;
        _playersProvider = playersProvider;
    }

    public void Initialize()
    {
        SetGameStage(ECurrentStageType.ChooseStartHandStage);
        SetCurrentActivePlayer(EPlayers.FirstPlayer);
        BeginStartHandStage();
    }

    private void BeginStartHandStage()
    {
        _currentPlayer = _playersProvider.GetPlayer(_currentPlayerType);

        for (int i = 0; i < 3; i++)
        {
            var card = _deckBuilder.GetTopCardFromDeck(_currentPlayerType);
            _currentPlayer.SetCardFromDeckInHand(card);
        }
    }

    private void SetGameStage(ECurrentStageType currentStage)
    {
        _currentStage = currentStage;
    }

    private void SetCurrentActivePlayer(EPlayers player)
    {
        _currentPlayerType = player;
    }

    public void Tick()
    {
        //todo: тут захардкожено. Убрать вообще отсюда
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     _currentDeck = _deckBuilder.GetFullDeck(_currentPlayerType);
        //     _deckBuilder.ShuffleDeck(_currentDeck);
        //     
        //     var card = _deckBuilder.GetTopCardFromDeck(_currentPlayerType);
        //     _cardMoverView.MoveCard(card);
        //     ChangeSide();
        // }
    }
    private void ChangeSide()
    {
        _currentPlayerType = _currentPlayerType == EPlayers.FirstPlayer ?
         EPlayers.SecondPlayer : EPlayers.FirstPlayer;
        switch (_currentPlayerType)
        {
            case EPlayers.FirstPlayer:
                _currentDeck = _deckBuilder.GetFullDeck(EPlayers.FirstPlayer);
                break;
            case EPlayers.SecondPlayer:
                _currentDeck = _deckBuilder.GetFullDeck(EPlayers.SecondPlayer);
                break;
        }
    }
}
