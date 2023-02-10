using System;
using Cards;
using Signals;
using UnityEngine;

namespace DefaultNamespace
{
    public class Abilities
    {
        private readonly CardSignalBus _cardSignalBus;


        public Abilities(CardSignalBus cardSignalBus)
        {
            _cardSignalBus = cardSignalBus;
        }

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

        public void ActivateCharge(CardView card)
        {
            card.CanAttack = true;
        }

        public void ActivateTaunt(CardView card)
        {
            card.IsTaunt = true;
        }

        public void DoBattlecry(CardView card)
        {
            _cardSignalBus.CardDoBattlecryFire(new CardDoBattlecrySignal(EBattlecrySubStage.True, card));

            switch (card.BattlecryParameters.Action)
            {
                case EBattlecryAction.None:
                    break;
                case EBattlecryAction.Deal:
                    DealDamageAbility(card);
                    break;
                case EBattlecryAction.Restore:
                    RestoreHpAbility(card);
                    break;
                case EBattlecryAction.Summon:
                    SummonUnitAbility(card);
                    break;
                case EBattlecryAction.Draw:
                    DrawCardAbility(card);
                    break;
                case EBattlecryAction.Buff:
                    OneTimeBuffAbility(card);
                    break;
            }
        }

        private void OneTimeBuffAbility(CardView card)
        {
            Debug.Log("OneTimeBuffAbility не реализован!!!");
            
            _cardSignalBus.CardDoBattlecryFire(new CardDoBattlecrySignal(EBattlecrySubStage.False, null));
        }

        private void DrawCardAbility(CardView card)
        {
            Debug.Log("DrawCardAbility");

            card.Owner.SetCardFromDeckInHand();

            _cardSignalBus.CardDoBattlecryFire(new CardDoBattlecrySignal(EBattlecrySubStage.False, null));
        }

        private void SummonUnitAbility(CardView card)
        {
            var player = card.Owner;
            var slot = player.GiveFirstFreeTableSlotForDrawAbility();
            if (slot == null)
            {
                Debug.Log("На столе нет свободного слота. Summon абилка не может быть выполнена");
                _cardSignalBus.CardDoBattlecryFire(new CardDoBattlecrySignal(EBattlecrySubStage.False, null));
                return;
            }

            var id = card.BattlecryParameters.SummonId;
            var dmg = card.BattlecryParameters.DMG;
            var hp = card.BattlecryParameters.HP;

            player.SummonCardOnTableFromBattlecry(id, dmg, hp, slot);

            _cardSignalBus.CardDoBattlecryFire(new CardDoBattlecrySignal(EBattlecrySubStage.False, null));
        }


        private void RestoreHpAbility(CardView card)
        {
            Debug.Log("RestoreHpAbility");

            switch (card.BattlecryParameters.Target)
            {
                case EBattlecryTarget.PointUnit:
                    _cardSignalBus.CardBattlecryClicker(new CardBattlecryClickerSignal(card));
                    break;
                case EBattlecryTarget.All:
                    foreach (var cardOnTable in card.Owner.MyCardsInTable)
                    {
                        if (cardOnTable == card) continue;
                        
                        cardOnTable.RestoreHp(card.BattlecryParameters.HP);
                    }
                    _cardSignalBus.CardDoBattlecryFire(new CardDoBattlecrySignal(EBattlecrySubStage.False, null));
                    break;
            }

           
        }

        private void DealDamageAbility(CardView card)
        {
            Debug.Log("DealDamageAbility");

            _cardSignalBus.CardBattlecryClicker(new CardBattlecryClickerSignal(card));
            //тут сигнал о завершении батлкрая приходит от дэмеджконтроллера, т.к. он наносит урон
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
    }
}