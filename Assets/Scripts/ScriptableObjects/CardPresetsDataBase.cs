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
                }

                return;
            }
        }
    }

    private void SetCardsForDeck()
    {
        foreach (var id in CardsId)
        {
            foreach (var cardPack in CardPacksContainer.CardPackConfigurations)
            {
                foreach (var cardPropertiesData in cardPack.Cards)
                {
                    if (id == cardPropertiesData.Id)
                    {
                        
                    }
                }
            }
        }
    }

    [ContextMenu("Проверить валидность собранной коллоды")]
    public void ValidateDeck()
    {
        bool isValid;
        isValid = ValidateId();
        isValid = isValid == true ? CheckHeroCards() : false;
        isValid = isValid == true ? CheckValidateCardsCount() : false;
        

        if (isValid)
        {
            SetCardsForDeck();
        }
    }

    private bool ValidateId()
    {
        foreach (var id in CardsId)
        {
            if (!CardPacksContainer.HasCardId(id))
            {
                return false;
            }
        }

        Debug.Log("Все Id валидные!");
        return true;
    }


    private bool CheckValidateCardsCount()
    {
        List<uint> resultList = removeDuplicates(CardsId);
        CardsId = resultList;
        if (CardsId.Count == 30)
        {
            Debug.Log($"Кол-во карт в колоде = {CardsId.Count}. Колода собрана");
            return true;
        }

        if (CardsId.Count < 30)
        {
            Debug.LogError($"Кол-во карт в колоде = {CardsId.Count} < 30. Добавьте {30 - CardsId.Count} карт(ы)!!!");
            return false;
        }

        Debug.LogError($"Кол-во карт в колоде = {CardsId.Count} > 30. Удалите {CardsId.Count - 30} карт(ы) из колоды!!!");
        return false;
    }


    private bool CheckHeroCards()
    {
        foreach (var id in CardsId)
        {
            if (CardPacksContainer.ValidHeroCard(id, Hero)) continue;
            Debug.LogError($"Карта с Id {id} не общая и не пренадлежит классу {Hero}!!!");
            return false;
        }

        Debug.Log("Карт чужых классов не обнаружено!");
        return true;
    }


    public static List<T> removeDuplicates<T>(List<T> list)
    {
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