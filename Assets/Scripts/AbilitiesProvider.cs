using System.Collections.Generic;
using Cards;
using UnityEngine;

namespace DefaultNamespace
{
    public class AbilitiesProvider
    {
        private ConfigCardsWithAbilities _configCardsWithAbilities;

        public AbilitiesProvider(ConfigCardsWithAbilities configCardsWithAbilities)
        {
            _configCardsWithAbilities = configCardsWithAbilities;
        }

        public void SetAbilitiesToCards(CardView card)
        {
           card.SetIncreaseStatsParameters(GetIncreaseStatsParameters(card));

           var abilitiesList = GetAbilities(card);
           if (abilitiesList.Count == 0)
           {
               return;
           }
           card.MyAbilities.AddRange(abilitiesList);
           
        }

        private List<EAbility> GetAbilities(CardView card)
        {
            var abilitiesList = new List<EAbility>();
            //заполняем карты с таунтом
            foreach (var id in _configCardsWithAbilities.IDCardsWithTauntConfig)
            {
                if (card.CardId == id) 
                {
                    abilitiesList.Add(EAbility.Taunt);
                }
            }
            //заполняем карты с чарджем
            foreach (var id in _configCardsWithAbilities.IDCardsWithChargeConfig)
            {
                if (card.CardId == id) 
                {
                    abilitiesList.Add(EAbility.Charge);
                }
            }

            return abilitiesList;

        }

        private IncreaseStatsParameters GetIncreaseStatsParameters(CardView card)
        {
            foreach (var parameters in _configCardsWithAbilities.IncreaseStatsConfig)
            {
                if (card.CardId == parameters.ID)
                {
                    return parameters;
                }
            }

            return null;
        }
        
        
    }
}