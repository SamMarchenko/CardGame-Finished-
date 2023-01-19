using System.Collections.Generic;
using Cards;
using DefaultNamespace;
using UnityEngine;
using Zenject;

public class GameCircle : IInitializable, ITickable
{
    private ECurrentStageType _currentStage;
    private EPlayers _currentActivePlayer = EPlayers.FirstPlayer;
    private readonly DeckManager _deckManager;
    private readonly CardMoveController _cardMoveController;
    private List<CardView> _currentDeck;
    
    

    public GameCircle(DeckManager deckManager, CardMoveController cardMoveController)
    {
        _deckManager = deckManager;
        _cardMoveController = cardMoveController;
    }

    public void Initialize()
    {
        SetGameStage(ECurrentStageType.ChooseStartHandStage);
        SetCurrentActivePlayer(EPlayers.FirstPlayer);
    }

    private void SetGameStage(ECurrentStageType currentStage)
    {
        _currentStage = currentStage;
    }

    private void SetCurrentActivePlayer(EPlayers player)
    {
        _currentActivePlayer = player;
    }

    public void Tick()
    {
        //todo: тут захардкожено. Убрать вообще отсюда
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var card = _deckManager.GetTopCardFromDeck(_currentActivePlayer);
            _cardMoveController.MoveCard(card);
            ChangeSide();
        }
    }
    private void ChangeSide()
    {
        _currentActivePlayer = _currentActivePlayer == EPlayers.FirstPlayer ?
         EPlayers.SecondPlayer : EPlayers.FirstPlayer;
        switch (_currentActivePlayer)
        {
            case EPlayers.FirstPlayer:
                _currentDeck = _deckManager.GetFullDeck(EPlayers.FirstPlayer);
                break;
            case EPlayers.SecondPlayer:
                _currentDeck = _deckManager.GetFullDeck(EPlayers.SecondPlayer);
                break;
        }
    }
}
