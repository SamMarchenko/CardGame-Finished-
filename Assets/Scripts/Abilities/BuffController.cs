using Cards;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class BuffController : ITickable
    {
        private readonly PlayersProvider _playersProvider;
        private readonly Abilities _abilities;
        private Player _firstPlayer;
        private Player _secondPlayer;

        public BuffController(Abilities abilities)
        {
            _abilities = abilities;
        }

        private void SetPlayer(Player player)
        {
            switch (player.PlayerType)
            {
                case EPlayers.FirstPlayer when _firstPlayer == null:
                    _firstPlayer = player;
                    return;
                case EPlayers.SecondPlayer when _secondPlayer == null:
                    _secondPlayer = player;
                    return;
            }
        }

        public void UpdateBuffersListForCardsOnTable(Player player)
        {
            SetPlayer(player);
            foreach (var buffer in player.MyBuffersInTable)
            {
                var bufferUnitType = buffer.IncreaseStatsParameters.UnitTypeBuff;
                foreach (var card in player.MyCardsInTable)
                {
                    if (card != buffer)
                    {
                        if (bufferUnitType == ECardUnitType.All || bufferUnitType == card.MyType)
                        {
                            if (!card.MyBuffers.Contains(buffer))
                            {
                                Debug.Log($"{card.NameText.text} добавился тут баффер!");
                                card.MyBuffers.Add(buffer);
                            }
                        }
                    }
                    // if (bufferUnitType == ECardUnitType.All || bufferUnitType == card.MyType && !card.MyBuffers.Contains(buffer))
                    // {
                    //     card.MyBuffers.Add(buffer);
                    // }


                    foreach (var myBuffer in card.MyBuffers)
                    {
                        if (!player.MyBuffersInTable.Contains(myBuffer))
                        {
                            card.MyBuffers.Remove(myBuffer);
                        }
                    }
                }
            }

            ActualizeBuffs(_firstPlayer);
            ActualizeBuffs(_secondPlayer);
        }


        public void Tick()
        {
            // ActualizeBuffs(_firstPlayer);
            // ActualizeBuffs(_secondPlayer);
        }

        private void ActualizeBuffs(Player player)
        {
            //todo: говно ебаное
        }
    }
}