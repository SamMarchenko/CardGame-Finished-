using System.Collections.Generic;
using Cards;
using UnityEngine;

namespace DefaultNamespace
{
    public class AbilitiesProvider
    {
        private ConfigCardsWithAbilities _configCardsWithAbilities;
        private List<BattlecryParameters> _cardsWithButtlecry;
        private readonly Abilities _abilities;
        private readonly BuffController _buffController;

        public AbilitiesProvider(ConfigCardsWithAbilities configCardsWithAbilities, Abilities abilities)
        {
            _configCardsWithAbilities = configCardsWithAbilities;
            _abilities = abilities;
            _cardsWithButtlecry = configCardsWithAbilities.BattlecriesConfig;
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

            foreach (var battlecry in _configCardsWithAbilities.BattlecriesConfig)
            {
                if (card.CardId == battlecry.ID)
                {
                    abilitiesList.Add(EAbility.Battlecry);
                }
            }
            //todo: остальные абилки добавлять сюда!

            return abilitiesList;
        }

        private BuffParameters GetIncreaseStatsParameters(CardView card)
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

        public void ActivateAbilitiesByThisCard(CardView card)
        {
            var abilitiesList = card.MyAbilities;


            if (abilitiesList.Contains(EAbility.IncreaseStats))
            {
                _abilities.DoBuffStats(card);
            }

            if (abilitiesList.Contains(EAbility.Charge))
            {
                _abilities.ActivateCharge(card);
            }

            if (abilitiesList.Contains(EAbility.Taunt))
            {
                _abilities.ActivateTaunt(card);
            }

            if (abilitiesList.Contains(EAbility.Battlecry))
            {
                AtivateButtlecry(card);
            }
        }

        public void DeactivateAbilitiesByThisCard(CardView card)
        {
            var abilitiesList = card.MyAbilities;


            if (abilitiesList.Contains(EAbility.IncreaseStats))
            {
                _abilities.DeleteBuffs(card);
            }
        }
        
        private void AtivateButtlecry(CardView card)
        {
            var cardsWithBC = _configCardsWithAbilities.BattlecriesConfig;

            foreach (var battlecriesParameters in cardsWithBC)
            {
                if (battlecriesParameters.ID == card.CardId)
                {
                    card.BattlecryParameters = battlecriesParameters;
                    break;
                }
            }
            Debug.Log("Вложил батлкрай в карту");

            _abilities.DoBattleCry(card);
        }
    }
}