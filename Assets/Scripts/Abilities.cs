using System;

namespace DefaultNamespace
{
    [Serializable]
    public class Abilities
    {
        public bool Taunt;
        public bool Rush;

        public bool Battlecry;
        private int _buttlecryId;
        public int ButtlectyId => _buttlecryId;

        public bool Ability;
        private int _abilityId;
        public int AbilityId => _abilityId;
    }
}