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
        
       var card = FindCardForID(id);

        if (card == null)
        {
            Debug.Log($"Id {id} не найден!!!");
            return false;
        }

        return true;
    }

    public bool ValidHeroCard(uint id, EHeroType hero)
    {
        {
            
        }
        return true;
    }

    private CardPropertiesData FindCardForID(uint id)
    {
        CardPropertiesData card = null;
        foreach (var cardPack in CardPackConfigurations)
        {
            foreach (var cardPropertiesData in cardPack.Cards)
            {
                if (id == cardPropertiesData.Id)
                {
                    card = cardPropertiesData;
                }
            }
        }
        return card;
    }
}