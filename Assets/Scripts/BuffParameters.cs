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
        public int ID = 0;
        [Width(80)]
        public int DamageBuff = 0;
        [Width(50)]
        public int HpBuff = 0;
        [Width(85)]
        public ECardUnitType UnitTypeBuff = ECardUnitType.All;
        
        
        
    }
}