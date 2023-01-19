using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using Cards.ScriptableObjects;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using Zenject;

public class GameCircle : IInitializable, ITickable
{
    private ECurrentStageType _currentStage;
    private EPlayers _currentActivePlayer = EPlayers.FirstPlayer;
    
    
    //private readonly DeckFactory _deckFactory;
    private readonly PlayerHandController _playerHandController;

    private readonly DeckManager _deckManager;
    // private CardView[] _playerDeck1;
    // private CardView[] _playerDeck2;
 

    private List<CardView> _currentDeck;
    
    

    public GameCircle(PlayerHandController playerHandController, DeckManager deckManager)
    {
        _playerHandController = playerHandController;
        _deckManager = deckManager;
    }

    public void Initialize()
    {
  //      _currentDeck = _playerDeck1;
    }

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
          //  GetCardFromDeck(_currentActivePlayer);
        }
    }

    // private void GetCardFromDeck(EPlayers player)
    // {
    //     for (int i = _currentDeck.Length-1; i >=0; i--)
    //     {
    //         if (_currentDeck[i] == null) continue;
    //         _playerHandController.SetNewCard(_currentActivePlayer, _currentDeck[i]);
    //         _currentDeck[i] = null;
    //         break;
    //     }
    //     ChangeSide();
    // }
    
    // private void ChangeSide()
    // {
    //     _currentActivePlayer = _currentActivePlayer == EPlayers.FirstPlayer ?
    //      EPlayers.SecondPlayer : EPlayers.FirstPlayer;
    //     switch (_currentActivePlayer)
    //     {
    //         case EPlayers.FirstPlayer:
    //             _currentDeck = _deckManager.GetFullDeck(EPlayers.FirstPlayer);
    //             break;
    //         case EPlayers.SecondPlayer:
    //             _currentDeck = _deckManager.GetFullDeck(EPlayers.SecondPlayer);
    //             break;
    //         default:
    //             throw new ArgumentOutOfRangeException();
    //     }
    // }
}
