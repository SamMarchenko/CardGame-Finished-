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
    private readonly DeckFactory _deckFactory;
    private readonly PlayerHandController _playerHandController;
    private CardView[] _playerDeck1;
    private CardView[] _playerDeck2;
    // private PlayerHandView _playerHand1;
    // private PlayerHandView _playerHand2;

    private CardView[] _currentDeck;
    private EPlayers _currentPlayer = EPlayers.First;
    

    public GameCircle(DeckFactory deckFactory, PlayerHandController playerHandController)
    {
        _deckFactory = deckFactory;
        _playerHandController = playerHandController;
    }

    public void Initialize()
    {
        _playerDeck1 = _deckFactory.CreateDeck(EPlayers.First);
        _playerDeck2 = _deckFactory.CreateDeck(EPlayers.Second);
        _currentDeck = _playerDeck1;

       
    }

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetCardFromDeck(_currentPlayer);
        }
    }

    private void GetCardFromDeck(EPlayers player)
    {
        for (int i = _currentDeck.Length-1; i >=0; i--)
        {
            if (_currentDeck[i] == null) continue;
            _playerHandController.SetNewCard(_currentPlayer, _currentDeck[i]);
            _currentDeck[i] = null;
            break;
        }
        ChangeSide();
    }
    
    private void ChangeSide()
    {
        _currentPlayer = _currentPlayer == EPlayers.First ? EPlayers.Second : EPlayers.First;
        switch (_currentPlayer)
        {
            case EPlayers.First:
                _currentDeck = _playerDeck1;
                break;
            case EPlayers.Second:
                _currentDeck = _playerDeck2;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
