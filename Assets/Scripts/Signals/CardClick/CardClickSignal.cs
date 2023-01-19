using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClickSignal
{
    public readonly CardView CardView;

    public CardClickSignal(CardView cardView)
    {
        CardView = cardView;
    }
}