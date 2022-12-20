using System.Collections;
using System.Collections.Generic;
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
    
}
