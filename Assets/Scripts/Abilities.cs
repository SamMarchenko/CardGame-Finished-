using System;
using Cards;

namespace DefaultNamespace
{
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


        public void DoIncreaseStats(CardView card)
        {
            var stats = card.IncreaseStatsParameters;

            switch (stats.UnitTypeBuff)
            {
                case ECardUnitType.All:
                    SetBuffersForCardsInTable(card, ECardUnitType.All);
                    break;
                case ECardUnitType.Murloc:
                    SetBuffersForCardsInTable(card, ECardUnitType.Murloc);
                    break;
                case ECardUnitType.Beast:
                    SetBuffersForCardsInTable(card, ECardUnitType.Beast);
                    break;
                case ECardUnitType.Elemental:
                    SetBuffersForCardsInTable(card, ECardUnitType.Elemental);
                    break;
                case ECardUnitType.Mech:
                    SetBuffersForCardsInTable(card, ECardUnitType.Beast);
                    break;
            }
        }

        public void ActivateCharge(CardView card)
        {
            card.CanAttack = true;
        }

        public void ActivateTaunt(CardView card)
        {
            card.IsTaunt = true;
        }

        private void SetBuffersForCardsInTable(CardView buffer, ECardUnitType unitType)
        {
            foreach (var card in buffer.Owner.MyCardsInTable)
            {
                if ((card == buffer) || (card.MyType != unitType && unitType != ECardUnitType.All) || (card.MyBuffers.Contains(buffer)))
                {
                    continue;
                }
                card.MyBuffers.Add(buffer);
            }
        }

        public void DoDecreaseStats(CardView card)
        {
            
        }
    }
}