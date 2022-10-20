using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private Transform[] _positions;
    private HandSlotsHandler _handSlotsHandler;
    private List<Card> _cards = new List<Card>();
    public List<Card> Cards => _cards;

    public void Init(HandSlotsHandler handSlotsHandler)
    {
        _handSlotsHandler = handSlotsHandler;
    }
    public void SetNewCard(ETurn turn ,Card card)
    {
        bool result = _handSlotsHandler.TrySetCardInHand(turn, card, out Transform slot);
        if (result)
        {
            _cards.Add(card);
            StartCoroutine(MoveInHand(card, slot));
        }
        else
        {
            Destroy(card.gameObject);
        }
    }

    public void RemoveCardFromHand(Card card)
    {
        _cards.Remove(card);
    }

    private IEnumerator MoveInHand(Card card, Transform slot)
    {
        var time = 0f;
        var startPos = card.transform.position;
        var endPos = slot.position;
        card.SwitchVisual();
        card.transform.eulerAngles = new Vector3(0,0,180);
        while (time < 1f)
        {
            card.transform.position = Vector3.Lerp(startPos, endPos, time);
            time += Time.deltaTime;
            yield return null;
        }
        card.transform.position = slot.position;
        card.CurrentPosition = card.transform.position;
        card.StateType = ECardStateType.InHand;
    }
    
}
