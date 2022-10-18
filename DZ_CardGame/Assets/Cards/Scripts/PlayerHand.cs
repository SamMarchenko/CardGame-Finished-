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
        if (_handSlotsHandler.isFreeSlot(turn))
        {
            var position = _handSlotsHandler.SetCardInSlot(card);
            _cards.Add(card);
            StartCoroutine(MoveInHand(card, position));
        }
        else
        {
            Destroy(card.gameObject);
        }
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
