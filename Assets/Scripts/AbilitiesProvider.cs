using System.Collections.Generic;
using Cards;
using UnityEngine;

namespace DefaultNamespace
{
    public class AbilitiesProvider
    {
        private ConfigCardsWithAbilities _configCardsWithAbilities;
        private readonly Abilities _abilities;

        public AbilitiesProvider(ConfigCardsWithAbilities configCardsWithAbilities, Abilities abilities)
        {
            _configCardsWithAbilities = configCardsWithAbilities;
            _abilities = abilities;
        }

        public void SetAbilityTypesToCards(CardView card)
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

        public void ActivateAbilities(CardView card)
        {
            var abilitiesList = card.MyAbilities;
            
            if (abilitiesList.Contains(EAbility.IncreaseStats))
            {
                _abilities.DoIncreaseStats(card);
            }

            if (abilitiesList.Contains(EAbility.Charge))
            {
                _abilities.ActivateCharge(card);
            }

            if (abilitiesList.Contains(EAbility.Taunt))
            {
                _abilities.ActivateTaunt(card);
            }
            
        }


        public void DeactivateAbilities(CardView card)
        {
            var abilitiesList = card.MyAbilities;

            if (abilitiesList.Contains(EAbility.IncreaseStats))
            {
                _abilities.DoDecreaseStats(card);
            }
        }
    }
}