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

        private Material _baseMat;
        private List<CardPropertiesData> _allCards;
        private int _maxNumberCardInDeck = 30;

        public DeckFactory(CardPropertiesDataProvider cardPropertiesDataProvider, CardView cardViewPrefab, ParentView parentView)
        {
            _cardPropertiesDataProvider = cardPropertiesDataProvider;
            _cardViewPrefab = cardViewPrefab;
            _parentView = parentView;
        }

        public CardView[] CreateDeck(EPlayers player)
        {
            var parent = player == EPlayers.First ? _parentView.Deck1Parent : _parentView.Deck2Parent;
            var deck = new CardView[_maxNumberCardInDeck];
            var offset = 0.8f;

            for (int i = 0; i < _maxNumberCardInDeck; i++)
            {
                deck[i] = MonoBehaviour.Instantiate(_cardViewPrefab, parent);
                deck[i].transform.localPosition = new Vector3(0f,offset,0f);
                deck[i].transform.eulerAngles = new Vector3(0,0,180f);
                deck[i].SwitchVisual();
                offset += 0.8f;
            
                //todo: тут заполняется рандомом, надо будет переделать по конкретную колоду
                var random = _allCards[Random.Range(0, _allCards.Count)];
            
                var newMat = new Material(_baseMat);
                newMat.mainTexture = random.Texture;
                deck[i].Configuration(random, CardUtility.GetDescriptionById(random.Id), newMat);
            }
        
            return deck;
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