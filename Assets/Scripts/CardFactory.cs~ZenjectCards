using System.Collections.Generic;
using Cards;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class DeckFactory : IInitializable
    {
        private readonly CardPropertiesDataProvider _cardPropertiesDataProvider;
        private readonly CardView _cardViewPrefab;
        private readonly ParentView _parentView;
        private readonly CardClickSignalHandler _clickSignalHandler;
        private readonly CardPointerSignalHandler _cardPointerSignalHandler;
        private readonly CardDragSignalHandler _cardDragSignalHandler;

        private Material _baseMat;
        private List<CardPropertiesData> _allCards;
        

        public DeckFactory(CardPropertiesDataProvider cardPropertiesDataProvider, CardView cardViewPrefab, ParentView parentView,
            CardClickSignalHandler cardClickSignalHandler, CardPointerSignalHandler cardPointerSignalHandler,
            CardDragSignalHandler cardDragSignalHandler)
        {
            _cardPropertiesDataProvider = cardPropertiesDataProvider;
            _cardViewPrefab = cardViewPrefab;
            _parentView = parentView;
            _clickSignalHandler = cardClickSignalHandler;
            _cardPointerSignalHandler = cardPointerSignalHandler;
            _cardDragSignalHandler = cardDragSignalHandler;
        }
        //
        // public List<CardView> CreateDeck(EPlayers player)
        // {
        //     var parent = player == EPlayers.FirstPlayer ? _parentView.Deck1Parent : _parentView.Deck2Parent;
        //     var deck = new List<CardView>(_maxNumberCardInDeck);
        //     var offset = 0.8f;
        //
        //     for (int i = 0; i < _maxNumberCardInDeck; i++)
        //     {
        //         deck.Add(MonoBehaviour.Instantiate(_cardViewPrefab, parent));
        //         deck[i].transform.localPosition = new Vector3(0f,offset,0f);
        //         deck[i].transform.eulerAngles = new Vector3(0,0,180f);
        //         deck[i].SwitchVisual();
        //         offset += 0.8f;
        //     
        //         //todo: тут заполняется рандомом, надо будет переделать по конкретную колоду
        //         var random = _allCards[Random.Range(0, _allCards.Count)];
        //     
        //         var newMat = new Material(_baseMat);
        //         newMat.mainTexture = random.Texture;
        //         deck[i].Init(_clickSignalHandler, _cardPointerSignalHandler, _cardDragSignalHandler);
        //         deck[i].Configuration(random, CardUtility.GetDescriptionById(random.Id), newMat);
        //     }
        //
        //     return deck;
        // }

        public CardView CreateCard()
        {
           var cardView = MonoBehaviour.Instantiate(_cardViewPrefab);
           
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

        public void Initialize()
        {
            FillAllCards();
        }
    }
}