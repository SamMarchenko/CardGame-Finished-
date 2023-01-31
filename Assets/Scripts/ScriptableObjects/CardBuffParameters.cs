using System.Collections.Generic;
using Cards;
using DefaultNamespace;
using OneLine;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "NewCardBuffParameters", menuName = "CardBuffs/Card Buffs")]
    public class CardBuffParameters : ScriptableObject
    {
        public ECardUnitType UnitType;

        [SerializeField, OneLine(Header = LineHeader.Short)]
        private BuffParameters[] _buffConfig;
        public BuffParameters[] BuffConfig => _buffConfig;
    }
}