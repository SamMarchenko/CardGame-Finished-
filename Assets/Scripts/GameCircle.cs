using System.Collections.Generic;
using Cards;
using DefaultNamespace;
using Signals;
using UnityEngine;
using Zenject;

public class GameCircle : IInitializable, ITickable, ICardDoBattlecryListener, IPlayerDeathListener
{
    private ECurrentStageType _currentStage = ECurrentStageType.StartWaiting;
    private EBattlecrySubStage _battlecrySubStage = EBattlecrySubStage.False;
    private EPlayers _currentPlayerType = EPlayers.FirstPlayer;
    private readonly DeckBuilder _deckBuilder;
    private readonly CardMoverView _cardMoverView;
    private readonly PlayersProvider _playersProvider;
    private readonly CardSignalBus _cardSignalBus;
    private readonly PlayerSignalBus _playerSignalBus;

    private Player _firstPlayer;
    private Player _secondPlayer;
    private Player _currentPlayer;
    
    

    public GameCircle(DeckBuilder deckBuilder, 
        CardMoverView cardMoverView, 
        PlayersProvider playersProvider, 
        CardSignalBus cardSignalBus, PlayerSignalBus playerSignalBus)
    {
        _deckBuilder = deckBuilder;
        _cardMoverView = cardMoverView;
        _playersProvider = playersProvider;
        _cardSignalBus = cardSignalBus;
        _playerSignalBus = playerSignalBus;
    }

    public void Initialize()
    {
        SetGameStage(ECurrentStageType.StartWaiting);
        GetPlayers();
        SetCurrentActivePlayer(EPlayers.FirstPlayer); 
    }

    private void GetPlayers()
    {
        _firstPlayer = _playersProvider.GetPlayer(EPlayers.FirstPlayer);
        _secondPlayer = _playersProvider.GetPlayer(EPlayers.SecondPlayer);
    }

    private void BeginStartHandStage()
    {
        _playerSignalBus.StageChangeFire(new StageChangeSignal(_currentStage));
        _playerSignalBus.CurrentPlayerChangeFire(new CurrentPlayerChangeSignal(_currentPlayerType, _currentPlayer));
        for (int i = 0; i < 3; i++)
        {
            _currentPlayer.SetCardFromDeckInHand();
        }
    }

    private void BeginMovingStage()
    {
        _playerSignalBus.StageChangeFire(new StageChangeSignal(_currentStage));
    }

    private void BeginGameOverStage()
    {
        _playerSignalBus.StageChangeFire(new StageChangeSignal(_currentStage));
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_currentStage == ECurrentStageType.StartWaiting)
            {
                SetGameStage(ECurrentStageType.ChooseStartHandStage);
                //для первого игрока
                BeginStartHandStage();
                return;
            }

            if (_currentStage == ECurrentStageType.ChooseStartHandStage && _currentPlayer == _firstPlayer)
            {
                Debug.Log("Игрок 1 выбрал стартовую руку");
                ChangeSide();
                //для второго игрока
                BeginStartHandStage();
                return;
            }

            if (_currentStage == ECurrentStageType.ChooseStartHandStage && _currentPlayer == _secondPlayer)
            {
                Debug.Log("Игрок 2 выбрал стартовую руку. Переходим в стадию дуэли!");
                SetGameStage(ECurrentStageType.MoveStage);
                BeginMovingStage();
                ChangeSide();
                return;
            }

            if (_currentStage == ECurrentStageType.MoveStage && _battlecrySubStage == EBattlecrySubStage.False)
            {
                Debug.Log("Ход окончен");
                ChangeSide();
            }
        }
    }
    private void ChangeSide()
    {
        _currentPlayerType = _currentPlayerType == EPlayers.FirstPlayer ?
         EPlayers.SecondPlayer : EPlayers.FirstPlayer;
        
        SetCurrentActivePlayer(_currentPlayerType);
        
        _playerSignalBus.CurrentPlayerChangeFire(new CurrentPlayerChangeSignal(_currentPlayerType, _currentPlayer));
    }

    public void OnCardDoBattlecry(CardDoBattlecrySignal signal)
    {
        _battlecrySubStage = signal.IsBattlecrySubStage;
        Debug.Log("Применяется баттлкрай в данный момент");
    }

    public void OnPlayerDeath(PlayerDeathSignal signal)
    {
        SetGameStage(ECurrentStageType.GameOver);
        BeginGameOverStage();
    }
}
