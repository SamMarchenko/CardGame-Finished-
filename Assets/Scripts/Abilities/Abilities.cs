using System;
using Cards;
using UnityEngine;

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
            SetBufferToPlayer(card);

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
                    SetBuffersForCardsInTable(card, ECardUnitType.Mech);
                    break;
            }
        }

        private void SetBufferToPlayer(CardView card)
        {
            if (!card.Owner.MyBuffersInTable.Contains(card))
            {
                card.Owner.MyBuffersInTable.Add(card);
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
                // if ((card == buffer) || (card.MyType != unitType && unitType != ECardUnitType.All) ||
                //     (card.MyBuffers.Contains(buffer)))
                // {
                //     continue;
                // }

                if (card == buffer)
                {
                    continue;
                }
                
                if (card.MyType != unitType && unitType != ECardUnitType.All)
                {
                    continue;
                }
                
                if (card.MyBuffers.Contains(buffer))
                {
                    continue;
                }
                Debug.Log($"{card.NameText.text} добавился тут баффер!");
                card.MyBuffers.Add(buffer);

                // if (card != buffer && (unitType == ECardUnitType.All || card.MyType == unitType) &&
                //     !card.MyBuffers.Contains(buffer))
                // {
                //     card.MyBuffers.Add(buffer);
                // }

                
            }
        }

        public void DoDecreaseStats(CardView card)
        {
        }
    }
}