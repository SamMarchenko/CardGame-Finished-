using System;
using Cards;
using UnityEngine;

namespace DefaultNamespace
{
    public class Abilities
    {
        public void DoBuffStats(CardView buffer)
        {
            SetBufferToPlayer(buffer);

            BuffAllCardsInTableByCurrentBuffer(buffer);
        }

        public void DeleteBuffs(CardView buffer)
        {
            DeleteBufferFromPlayer(buffer);
            CancelBuffFromAllCardsInTableByCurrentBuffer(buffer);
        }

        private void CancelBuffFromAllCardsInTableByCurrentBuffer(CardView buffer)
        {
            foreach (var card in buffer.Owner.MyCardsInTable)
            {
                UnBuffCard(card, buffer);
            }
        }

        private void UnBuffCard(CardView card, CardView buffer)
        {
            if (buffer.BuffStatsParameters.UnitTypeBuff == ECardUnitType.All ||
                card.MyType == buffer.BuffStatsParameters.UnitTypeBuff)
            {
                DeactivateBuffOnCard(card, buffer);
            }
        }

        private void DeactivateBuffOnCard(CardView card, CardView buffer)
        {
            var buffDmg = -1 * buffer.BuffStatsParameters.DamageBuff;
            var buffHp = -1 * buffer.BuffStatsParameters.HpBuff;
            if (buffDmg != 0)
            {
                var currentDmg = card.GetDamage() + buffDmg;
                card.SetDamage(currentDmg, buffDmg);
            }

            if (buffHp != 0)
            {
                var currentHp = card.GetHealth() + buffHp;
                if (currentHp <= 0)
                {
                    currentHp = 1;
                }

                card.SetHealth(currentHp, buffHp);
            }
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
                if (buffer.BuffStatsParameters.UnitTypeBuff == ECardUnitType.All ||
                    card.MyType == buffer.BuffStatsParameters.UnitTypeBuff)
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

        private void DeleteBufferFromPlayer(CardView card)
        {
            if (card.Owner.MyBuffersInTable.Contains(card))
            {
                card.Owner.MyBuffersInTable.Remove(card);
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