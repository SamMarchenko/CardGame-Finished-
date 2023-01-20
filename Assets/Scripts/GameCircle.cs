using System.Collections.Generic;
using Cards;
using DefaultNamespace;
using UnityEngine;
using Zenject;

public class GameCircle : IInitializable, ITickable
{
    private ECurrentStageType _currentStage;
    private EPlayers _currentActivePlayer = EPlayers.FirstPlayer;
    private readonly CardsController _cardsController;
    private readonly CardMoverView _cardMoverView;
    private List<CardView> _currentDeck;
    
    

    public GameCircle(CardsController cardsController, CardMoverView cardMoverView)
    {
        _cardsController = cardsController;
        _cardMoverView = cardMoverView;
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
            var card = _cardsController.GetTopCardFromDeck(_currentActivePlayer);
            _cardMoverView.MoveCard(card);
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
                _currentDeck = _cardsController.GetFullDeck(EPlayers.FirstPlayer);
                break;
            case EPlayers.SecondPlayer:
                _currentDeck = _cardsController.GetFullDeck(EPlayers.SecondPlayer);
                break;
        }
    }
}
