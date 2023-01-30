using System;
using OneLine;
using UnityEngine;

namespace Cards
{
    [Serializable]
    public class CardPropertiesData
    {
        [Width(40)]
        public uint Id;
        [Width(30)]
        public ushort Cost;
        public string Name;
        [Width(50)]
        public Texture Texture;
        [Width(40)]
        public ushort Attack;
        [Width(40)]
        public ushort Health;
        [Width(65)]
        public ECardUnitType Type;
        [Width(35)]
        public bool Taunt = false;
        [Width(45)]
        public bool Charge = false;
        [Width(55)]
        public bool Battlecry = false;
        [Width(40)]
        public bool Ability = false;
        
        
        public CardParamsData GetParams()
        {
            return new CardParamsData(Cost, Attack, Health);
        }
    }
}