using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    [SerializeField] private Transform _deckPosition;
    public Transform DeckPosition => _deckPosition;
    private List<Card> _cards;

    public void SetCards(List<Card> cards)
    {
        _cards = cards;
    }

    public void SetCard(Card card)
    {
        _cards.Add(card);
    }

    public void RemoveCard(Card card)
    {
        _cards.Remove(card);
    }

    public List<Card> GetCards()
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
        for (int i = _cards.Count - 1; i >= 1; i--)
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
            if (card is null)
            {
                Debug.Log("null");
            }
            card.transform.localPosition = new Vector3(0f, offset, 0f);
            card.transform.eulerAngles = new Vector3(0, 0, 180f);
            offset += 0.8f;
        }
    }
}