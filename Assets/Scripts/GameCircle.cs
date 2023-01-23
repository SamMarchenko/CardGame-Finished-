using System.Collections.Generic;
using Cards;
using DefaultNamespace;
using Signals;
using UnityEngine;
using Zenject;

public class GameCircle : IInitializable, ITickable
{
    private ECurrentStageType _currentStage = ECurrentStageType.StartWaiting;
    private EPlayers _currentPlayerType = EPlayers.FirstPlayer;
    private readonly DeckBuilder _deckBuilder;
    private readonly CardMoverView _cardMoverView;
    private readonly PlayersProvider _playersProvider;
    private readonly CardSignalBus _bus;

    private List<CardView> _currentDeck;
    private Player _firstPlayer;
    private Player _secondPlayer;
    private Player _currentPlayer;
    
    

    public GameCircle(DeckBuilder deckBuilder, 
        CardMoverView cardMoverView, 
        PlayersProvider playersProvider, 
        CardSignalBus bus)
    {
        _deckBuilder = deckBuilder;
        _cardMoverView = cardMoverView;
        _playersProvider = playersProvider;
        _bus = bus;

    }

    public void Initialize()
    {
        SetGameStage(ECurrentStageType.StartWaiting);
        GetPlayers();
        SetCurrentActivePlayer(EPlayers.FirstPlayer);
        //BeginStartHandStage();
    }

    private void GetPlayers()
    {
        _firstPlayer = _playersProvider.GetPlayer(EPlayers.FirstPlayer);
        _secondPlayer = _playersProvider.GetPlayer(EPlayers.SecondPlayer);
    }

    private void BeginStartHandStage()
    {
        _bus.StageChangeFire(new StageChangeSignal(_currentStage));
        _bus.CurrentPlayerChangeFire(new CurrentPlayerChangeSignal(_currentPlayerType));
        for (int i = 0; i < 3; i++)
        {
            var card = _deckBuilder.GetTopCardFromDeck(_currentPlayer);
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
        
        _currentPlayer = _currentPlayerType == EPlayers.FirstPlayer ? _firstPlayer : _secondPlayer;
    }

    public void Tick()
    {
        //todo: тут захардкожено. Убрать вообще отсюда
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_currentStage == ECurrentStageType.StartWaiting)
            {
                SetGameStage(ECurrentStageType.ChooseStartHandStage);
                BeginStartHandStage();
                return;
            }
            Debug.Log("Ход окончен");
            ChangeSide();
        }
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
        _bus.CurrentPlayerChangeFire(new CurrentPlayerChangeSignal(_currentPlayerType));
    }
}
