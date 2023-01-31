using System;
using Cards;
using OneLine;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public class BuffParameters
    {
        [Width(50)]
        public int _cardId;
        [Width(80)]
        public int _damageBuff;
        [Width(50)]
        public int _hpBuff;
        [Width(85)]
        public ECardUnitType _unitTypeBuff;
    }
}