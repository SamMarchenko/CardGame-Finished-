using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using OneLine;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/CardPresets")]
public class CardPresetsDataBase : ScriptableObject
{
    [SerializeField] private CardPacksContainer CardPacksContainer;

    public EHeroType Hero;

    public List<uint> CardsId = new List<uint>();
    private List<CardPropertiesData> _cards = new List<CardPropertiesData>();

    [ContextMenu("Set Class Cards")]
    public void SetClassCards()
    {
        foreach (var pack in CardPacksContainer.CardPackConfigurations)
        {
            if (Hero != EHeroType.Common && pack.HeroType == Hero)
            {
                ClearCards();
                foreach (var cardPropertiesData in pack.Cards)
                {
                    CardsId.Add(cardPropertiesData.Id);
                    _cards.Add(cardPropertiesData);
                }

                return;
            }
        }
    }

    [ContextMenu("Проверить валидность собранной коллоды")]
    public void ValidateDeck()
    {
        ValidateId();
        CheckValidateCardsCount();
        CheckHeroCards();
    }
    
    private void ValidateId()
    {
        foreach (var id in CardsId)
        {
            bool isValid = false;
            foreach (var cardPack in CardPacksContainer.CardPackConfigurations)
            {
                foreach (var cardPropertiesData in cardPack.Cards)
                {
                    if (id == cardPropertiesData.Id)
                    {
                        isValid = true;
                    }
                }
            }

            if (!isValid)
            {
                Debug.Log($"Id {id} не найден!!!");
                return;
            }
        }
        Debug.Log("Все Id валидные!");
    }

    
    private void CheckValidateCardsCount()
    {
        List<uint> resultList = removeDuplicates(CardsId);
        CardsId = resultList;
        if (CardsId.Count == 30)
        {
            Debug.Log($"Кол-во карт в колоде = {CardsId.Count}. Колода собрана");
        }
        else if (CardsId.Count < 30)
        {
            Debug.Log($"Кол-во карт в колоде = {CardsId.Count} < 30. Доберите колоду!!!");
        }
        else
        {
            Debug.Log($"Кол-во карт в колоде = {CardsId.Count} > 30. Удалите лишние карты из колоды!!!");
        }
    }
    

    private void CheckHeroCards()
    {
        foreach (var id in CardsId)
        {
            bool isValid = true;
            EHeroType heroType = EHeroType.Common;
            foreach (var cardPack in CardPacksContainer.CardPackConfigurations)
            {
                foreach (var cardPropertiesData in cardPack.Cards)
                {
                    if (id == cardPropertiesData.Id)
                    {
                        if (cardPack.HeroType != EHeroType.Common && Hero != cardPack.HeroType)
                        {
                            isValid = false;
                            heroType = cardPack.HeroType;
                        }
                    }
                }
            }

            if (!isValid)
            {
                Debug.Log($"Карта с Id {id} пренадлежит классу {heroType}, а герой класса {Hero}!!!");
                return;
            }
        }
        Debug.Log("Карт чужых классов не обнаружено!");
    }
    
    
    
    public static List<T> removeDuplicates<T>(List<T> list) {
        return new HashSet<T>(list).ToList();
    }

    private void ClearCards()
    {
        CardsId.Clear();
        CardsId.Capacity = 0;
        _cards.Clear();
        _cards.Capacity = 0;
    }
}