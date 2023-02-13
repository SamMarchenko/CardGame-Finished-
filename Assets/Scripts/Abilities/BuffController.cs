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

        public BuffController(Abilities abilities)
        {
            _abilities = abilities;
        }

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
    }
}