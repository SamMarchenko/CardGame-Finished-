using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    private Card[] _cards;
    [SerializeField] private Transform[] _positions;

    private void Start()
    private List<Card> _cards = new List<Card>();
    public List<Card> Cards => _cards;
    }
    
    public bool SetNewCard(Card card)
    {
        //var result = GetLastPosition();
        if (_cards.Count == 10)
        {
            Destroy(card.gameObject);
            return false;
        }

        _cards[result] = card;
        StartCoroutine(MoveInHand(card,_positions[result]));
        return true;
    }

    private IEnumerator MoveInHand(Card card, Transform parent)
    {
        var time = 0f;
        var startPos = card.transform.position;
        var endPos = parent.position;
        card.SwitchVisual();
        card.transform.eulerAngles = new Vector3(0,0,180);
        while (time < 1f)
        {
            card.transform.position = Vector3.Lerp(startPos, endPos, time);
            time += Time.deltaTime;
            yield return null;
        }
        card.transform.position = parent.position;
        card.CurrentPosition = card.transform.position;
        card.StateType = ECardStateType.InHand;
    }

    // private int GetLastPosition()
    // {
    //     for (int i = 0; i < _cards.Length; i++)
    //     {
    //         if (_cards[i] == null) return i;
    //     }
    //     return -1;
    // }
    {
        for (int i = 0; i < _cards.Length; i++)
        {
            if (_cards[i] == null) return i;
            
        }
        return -1;
    }
}
