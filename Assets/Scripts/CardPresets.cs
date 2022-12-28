using System;
using Cards;
using OneLine;
using UnityEngine;

[Serializable]
public class CardPresets
{
    public EHeroType Hero;
    public uint[] Cards = new uint[30];
}
