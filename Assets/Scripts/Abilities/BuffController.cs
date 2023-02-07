using System.Linq;
using Cards;
using ModestTree;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class BuffController
    {
        private readonly PlayersProvider _playersProvider;
        private readonly Abilities _abilities;
       // private Player _firstPlayer;
       // private Player _secondPlayer;

        public BuffController(Abilities abilities)
        {
            _abilities = abilities;
        }

        // private void SetPlayer(Player player)
        // {
        //     switch (player.PlayerType)
        //     {
        //         case EPlayers.FirstPlayer when _firstPlayer == null:
        //             _firstPlayer = player;
        //             return;
        //         case EPlayers.SecondPlayer when _secondPlayer == null:
        //             _secondPlayer = player;
        //             return;
        //     }
        // }

        // public void UpdateBuffsOnTable(Player player)
        // {
        //     SetPlayer(player);
        //     // RemoveBuffersFromCardsOnTable(player);
        //     // AddBuffersToCardsOnTable(player);
        // }

        public void CheckAndGiveBuffToThisCard(CardView card)
        {
            var playersBuffers = card.Owner.MyBuffersInTable;
            if (playersBuffers.IsEmpty())
            {
                return;
            }
            foreach (var buffer in playersBuffers)
            {
                _abilities.BuffCard(card, buffer);
            }
        }

      

        

        private void DeactivateBuffToAllCardsOnTable(Player player, CardView buffer)
        {
            foreach (var card in player.MyCardsInTable)
            {
                DeactivateBuffToCard(card, buffer);
            }
        }

        

        private void DeactivateBuffToCard(CardView card, CardView buffer)
        {
            var dmg = 0;
            var hp = 0;
            var dmgBuff = buffer.BuffStatsParameters.DamageBuff;
            var hpBuff = buffer.BuffStatsParameters.HpBuff;

            dmg = card.GetDamage() - dmgBuff;
            hp = card.GetHealth() - hpBuff;

            card.SetDamage(dmg, -dmgBuff);
            card.SetHealth(hp, -hpBuff);
        }
    }
}