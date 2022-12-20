using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    private CardView[] _cards;
    [SerializeField] private Transform[] _positions;

    private void Start()
    {
        _cards = new CardView[_positions.Length];
    }

    public bool SetNewCard(CardView cardView)
    {
        var result = GetLastPosition();
        if (result == -1)
        {
            Destroy(cardView.gameObject);
            return false;
        }

        _cards[result] = cardView;
        StartCoroutine(MoveInHand(cardView,_positions[result]));
        return true;
    }

    private IEnumerator MoveInHand(CardView cardView, Transform parent)
    {
        var time = 0f;
        var startPos = cardView.transform.position;
        var endPos = parent.position;
        cardView.SwitchVisual();
        cardView.transform.eulerAngles = new Vector3(0,0,180);
        while (time < 1f)
        {
            cardView.transform.position = Vector3.Lerp(startPos, endPos, time);
            time += Time.deltaTime;
            yield return null;
        }
        cardView.transform.parent = parent;
        cardView.StateType = ECardStateType.InHand;
    }

    private int GetLastPosition()
    {
        for (int i = 0; i < _cards.Length; i++)
        {
            if (_cards[i] == null) return i;
            
        }
        return -1;
    }
}
