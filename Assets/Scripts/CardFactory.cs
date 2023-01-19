using System.Collections.Generic;
using Cards;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class CardFactory : IInitializable
    {
        private readonly CardPropertiesDataProvider _cardPropertiesDataProvider;
        private readonly CardView _cardViewPrefab;
        private readonly CardClickSignalHandler _clickSignalHandler;
        private readonly CardPointerSignalHandler _cardPointerSignalHandler;
        private readonly CardDragSignalHandler _cardDragSignalHandler;

        private Material _baseMat;
        private List<CardPropertiesData> _allCards;

        public CardFactory(
            CardPropertiesDataProvider cardPropertiesDataProvider,
            CardView cardViewPrefab,
            CardClickSignalHandler cardClickSignalHandler,
            CardPointerSignalHandler cardPointerSignalHandler,
            CardDragSignalHandler cardDragSignalHandler
            )
        {
            _cardPropertiesDataProvider = cardPropertiesDataProvider;
            _cardViewPrefab = cardViewPrefab;
            _clickSignalHandler = cardClickSignalHandler;
            _cardPointerSignalHandler = cardPointerSignalHandler;
            _cardDragSignalHandler = cardDragSignalHandler;
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
            cardView.Init(_clickSignalHandler, _cardPointerSignalHandler, _cardDragSignalHandler);
            cardView.Configuration(random, CardUtility.GetDescriptionById(random.Id), newMat);

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