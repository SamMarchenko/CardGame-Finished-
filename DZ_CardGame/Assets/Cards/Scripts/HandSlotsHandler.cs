using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;

public class HandSlotsHandler : MonoBehaviour
{
    [SerializeField] private DrawCardSlots[] _player1HandSlots;
    [SerializeField, Space] private DrawCardSlots[] _player2HandSlots;
    private DrawCardSlots _freeSlot;


    public bool isFreeSlot(ETurn turn)
    {
        switch (turn)
        {
            case ETurn.First:
                return CheckSlots(_player1HandSlots);
            case ETurn.Second:
                return CheckSlots(_player2HandSlots);
            default:
                throw new ArgumentOutOfRangeException(nameof(turn), turn, null);
        }
    }

    public Transform SetCardInSlot(Card card)
    {
        _freeSlot.SlotStatus = ESlotStatus.Busy;
        _freeSlot.SetCard(card);
        return _freeSlot.transform;
    }

    public DrawCardSlots FindSlotByCard(ETurn turn, Card card)
    {
        var slot = new DrawCardSlots();
        if (turn == ETurn.First)
        {
            slot = FindSlot(_player1HandSlots, card);
        }
        else
        {
            slot = FindSlot(_player2HandSlots, card);
        }

        return slot;
    }

    public void RefreshSlot(DrawCardSlots slot)
    {
        slot.SlotStatus = ESlotStatus.Free;
    }

    private bool CheckSlots(DrawCardSlots[] slots)
    {
        foreach (var drawCardSlot in slots)
        {
            if (drawCardSlot.SlotStatus == ESlotStatus.Free)
            {
                _freeSlot = drawCardSlot;
                return true;
            }
        }

        _freeSlot = null;
        return false;
    }

    private DrawCardSlots FindSlot(DrawCardSlots[] slots, Card card)
    {
        foreach (var drawCardSlot in slots)
        {
            if (drawCardSlot.CardInSlot == card)
            {
                return drawCardSlot;
            }
        }

        return default;
    }
}