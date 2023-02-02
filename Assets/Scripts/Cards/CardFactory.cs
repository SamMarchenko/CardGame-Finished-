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
        private readonly CardSignalBus _bus;
        private readonly AbilitiesProvider _abilitiesProvider;


        private Material _baseMat;
        private List<CardPropertiesData> _allCards;

        public CardFactory(
            CardPropertiesDataProvider cardPropertiesDataProvider,
            CardView cardViewPrefab,
            CardSignalBus bus,
            AbilitiesProvider abilitiesProvider
        )
        {
            _cardPropertiesDataProvider = cardPropertiesDataProvider;
            _cardViewPrefab = cardViewPrefab;
            _bus = bus;
            _abilitiesProvider = abilitiesProvider;
        }

        public void Initialize()
        {
            FillAllCards();
        }

        public CardView CreateCard(Transform parent)
        {
            var cardView = MonoBehaviour.Instantiate(_cardViewPrefab, parent);

            var random = _allCards[Random.Range(0, _allCards.Count)];
            var newMat = new Material(_baseMat);
            newMat.mainTexture = random.Texture;
            cardView.Init(_bus);
            cardView.Configuration(random, CardUtility.GetDescriptionById(random.Id), newMat);
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