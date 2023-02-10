using System.Collections.Generic;
using Cards;
using Signals;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class CardFactory : IInitializable
    {
        private readonly CardPropertiesDataProvider _cardPropertiesDataProvider;
        private readonly CardView _cardViewPrefab;
        private readonly CardSignalBus _cardSignalBus;
        private readonly AbilitiesProvider _abilitiesProvider;


        private Material _baseMat;
        private List<CardPropertiesData> _allCards;
        private List<uint> _firstPlayerID = new List<uint>(30);

        public CardFactory(
            CardPropertiesDataProvider cardPropertiesDataProvider,
            CardView cardViewPrefab,
            CardSignalBus cardSignalBus,
            AbilitiesProvider abilitiesProvider
        )
        {
            _cardPropertiesDataProvider = cardPropertiesDataProvider;
            _cardViewPrefab = cardViewPrefab;
            _cardSignalBus = cardSignalBus;
            _abilitiesProvider = abilitiesProvider;
        }

        public void Initialize()
        {
            FillAllCards();
        }

        public void SetFirstPlayerCardsID(uint[] idArray)
        {
            foreach (var id in idArray)
            {
                _firstPlayerID.Add(id);
            }
        }

        public CardView CreateCard(Transform parent, EPlayers playerType)
        {
            var cardView = MonoBehaviour.Instantiate(_cardViewPrefab, parent);
            var newMat = new Material(_baseMat);
            if (playerType == EPlayers.FirstPlayer)
            {
                foreach (var cardPropertiesData in _allCards)
                {
                    if (_firstPlayerID.Contains(cardPropertiesData.Id))
                    {
                        var data = cardPropertiesData;
                        newMat.mainTexture = data.Texture;
                        cardView.Init(_cardSignalBus);
                        cardView.Configuration(data, CardUtility.GetDescriptionById(data.Id), newMat);
                        _abilitiesProvider.SetAbilityTypesToCards(cardView);

                        _firstPlayerID.Remove(cardPropertiesData.Id);
                        break;
                    }
                }

                return cardView;
            }

            var random = _allCards[Random.Range(0, _allCards.Count)];
            newMat.mainTexture = random.Texture;
            cardView.Init(_cardSignalBus);
            cardView.Configuration(random, CardUtility.GetDescriptionById(random.Id), newMat);
            _abilitiesProvider.SetAbilityTypesToCards(cardView);

            return cardView;
        }

        public CardView SummonCardForAbility(int id, Transform slot)
        {
            var cardView = MonoBehaviour.Instantiate(_cardViewPrefab, slot);
            var cardData = new CardPropertiesData();
            foreach (var cardPropertiesData in _allCards)
            {
                if (cardPropertiesData.Id == (uint)id)
                {
                    cardData = cardPropertiesData;
                    break;
                }
            }
            var newMat = new Material(_baseMat);
            newMat.mainTexture = cardData.Texture;
            
            cardView.Init(_cardSignalBus);
            cardView.Configuration(cardData, CardUtility.GetDescriptionById(cardData.Id), newMat);
            _abilitiesProvider.SetAbilityTypesToCards(cardView);
            
            
            return cardView;
        }


        private void FillAllCards()
        {
            _allCards = _cardPropertiesDataProvider.GetAllCards();
            _baseMat = new Material(Shader.Find("TextMeshPro/Sprite"));
            _baseMat.renderQueue = 2997;
        }
    }
}