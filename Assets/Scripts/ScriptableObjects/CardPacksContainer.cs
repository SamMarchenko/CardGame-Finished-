using System.Collections;
using System.Collections.Generic;
using Cards;
using Cards.ScriptableObjects;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "CardPackConfigurations", menuName = "Common Resources Container")]
public class CardPacksContainer : ScriptableObject
{
    [SerializeField] private CardPackConfiguration[] _cardPackConfigurations;
    public CardPackConfiguration[] CardPackConfigurations => _cardPackConfigurations;

    public void InstallBindings(DiContainer container)
    {
        foreach (var cardPack in _cardPackConfigurations)
        {
            container.BindInstance(cardPack);
        }
    }

    public bool HasCardId(uint id)
    {
        var card = FindCardForID(id, out var heroType);

        if (card == null)
        {
            Debug.LogError($"Id {id} не найден!!!");
            return false;
        }

        return true;
    }

    public List<int> GetCardsWithAbilityType(EAbility ability)
    {
        return FindCardsForAbility(ability);
    }

    

    public bool ValidHeroCard(uint id, EHeroType hero)
    {
        FindCardForID(id, out var heroType);

        return heroType == hero || heroType == EHeroType.Common;
    }

    private CardPropertiesData FindCardForID(uint id, out EHeroType heroType)
    {
        CardPropertiesData card = null;
        
        foreach (var cardPack in CardPackConfigurations)
        {
            heroType = cardPack.HeroType;
            foreach (var cardPropertiesData in cardPack.Cards)
            {
                if (id == cardPropertiesData.Id)
                {
                    card = cardPropertiesData;
                    return card;
                }
            }
        }
        heroType = EHeroType.Common;
        return card;
    }

    private List<int> FindCardsForAbility(EAbility ability)
    {
        var cardId = new List<int>();
        
        foreach (var cardPack in CardPackConfigurations)
        {
            foreach (var cardPropertiesData in cardPack.Cards)
            {
                if (cardPropertiesData.Ability == ability)
                {
                    cardId.Add((int)cardPropertiesData.Id);
                }
            }
        }
        return cardId;
    }
    
    
}