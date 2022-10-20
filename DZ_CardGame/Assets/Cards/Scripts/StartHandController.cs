using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartHandController
{
    private Card[] _player1Deck;
    private Card[] _player2Deck;

    public StartHandController(Card[] player1Deck, Card[] player2Deck)
    {
        _player1Deck = player1Deck;
        _player2Deck = player2Deck;
    }
}
