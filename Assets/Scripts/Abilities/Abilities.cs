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


        public void DoBuffStats(CardView buffer)
        {
            SetBufferToPlayer(buffer);

            BuffAllCardsInTableByCurrentBuffer(buffer);
        }

        private void BuffAllCardsInTableByCurrentBuffer(CardView buffer)
        {
            foreach (var card in buffer.Owner.MyCardsInTable)
            {
                BuffCard(card, buffer);
            }
        }

        public void BuffCard(CardView card, CardView buffer)
        {
            if (card != buffer)
            {
                if (buffer.BuffStatsParameters.UnitTypeBuff == ECardUnitType.All || card.MyType == buffer.BuffStatsParameters.UnitTypeBuff)
                {
                    ActivateBuffOnCard(card, buffer);
                }
            }
        }

        private void ActivateBuffOnCard(CardView card, CardView buffer)
        {
            var buffDmg = buffer.BuffStatsParameters.DamageBuff;
            var buffHp = buffer.BuffStatsParameters.HpBuff;
            if (buffDmg > 0)
            {
                var currentDmg = card.GetDamage() + buffDmg;
                card.SetDamage(currentDmg, buffDmg);
            }

            if (buffHp > 0)
            {
                var currentHp = card.GetHealth() + buffHp;
                card.SetHealth(currentHp, buffHp);
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
    }
}