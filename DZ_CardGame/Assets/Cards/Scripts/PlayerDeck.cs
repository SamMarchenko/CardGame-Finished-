using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    private Card[] _cards;

    public void SetCards(Card[] cards)
    {
        _cards = cards;
    }

    public Card[] GetCards()
    {
        return _cards;
    }
}
