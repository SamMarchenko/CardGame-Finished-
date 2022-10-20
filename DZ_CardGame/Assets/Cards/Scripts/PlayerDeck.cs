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

    public void MixDeck()
    {
        MixCardsInDeck();
        LineUpCardsStack();
    }

    private void MixCardsInDeck()
    {
        for (int i = _cards.Length - 1; i >= 1; i--)
        {
            var j = Random.Range(0, i + 1);
            var temp = _cards[j];
            _cards[j] = _cards[i];
            _cards[i] = temp;
        }
    }

    private void LineUpCardsStack()
    {
        var offset = 0.8f;
        foreach (var card in _cards)
        {
            card.transform.localPosition = new Vector3(0f, offset, 0f);
            card.transform.eulerAngles = new Vector3(0, 0, 180f);
            offset += 0.8f;
        }
    }
}