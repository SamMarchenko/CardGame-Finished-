using System.Collections.Generic;
using Cards;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace DefaultNamespace
{
    public class DeckBuilder //: IInitializable
    {
        private readonly CardFactory _cardFactory;
        private readonly ParentView _parentView;
        private readonly CardPresetsDataBase _cardPresetsDataBase;
        private List<CardView> _playerDeck1;
        private List<CardView> _playerDeck2;
        private int _maxNumberCardInDeck = 30;
        private Random _random;
        private uint[] _firstPlayerCardsId = new uint[30];

        public DeckBuilder(CardFactory cardFactory, ParentView parentView, CardPresetsDataBase cardPresetsDataBase)
        {
            _cardFactory = cardFactory;
            _parentView = parentView;
            _cardPresetsDataBase = cardPresetsDataBase;
            
            _random = new Random();
            
            for (var i = 0; i < _firstPlayerCardsId.Length; i++)
            {
                _firstPlayerCardsId[i] = _cardPresetsDataBase.CardsId[i];
            }
        }

        // public void Initialize()
        // {
        //     // _playerDeck1 = BuildDeck(EPlayers.FirstPlayer);
        //     // _playerDeck2 = BuildDeck(EPlayers.SecondPlayer);
        //     _random = new Random();
        // }

        private bool IsDecksBuilded()
        {
            return _playerDeck1 != null && _playerDeck2 != null;
        }

        private List<CardView> BuildDeck(EPlayers player)
        {
            var parent = player == EPlayers.FirstPlayer ? _parentView.Deck1Parent : _parentView.Deck2Parent;
            var deck = new List<CardView>(_maxNumberCardInDeck);
            var offset = 0.8f;

            if (player == EPlayers.FirstPlayer)
            {
                _cardFactory.SetFirstPlayerCardsID(_firstPlayerCardsId);

                for (var i = 0; i < _firstPlayerCardsId.Length; i++)
                {
                    deck.Add(_cardFactory.CreateCard(parent, player));
                    deck[i].transform.localPosition = new Vector3(0f, offset, 0f);
                    deck[i].transform.eulerAngles = new Vector3(0, 0, 180f);
                    deck[i].SwitchVisual();
                    offset += 0.8f;
                }
                ShuffleDeck(deck);
                return deck;
            }

            for (int i = 0; i < _maxNumberCardInDeck; i++)
            {
                deck.Add(_cardFactory.CreateCard(parent, player));
                deck[i].transform.localPosition = new Vector3(0f, offset, 0f);
                deck[i].transform.eulerAngles = new Vector3(0, 0, 180f);
                deck[i].SwitchVisual();
                offset += 0.8f;
            }

            return deck;
        }

        public void ShuffleDeck(List<CardView> deck)
        {
            for (var i = deck.Count - 1; i >= 1; i--)
            {
                var j = _random.Next(i + 1);
                (deck[j], deck[i]) = (deck[i], deck[j]);
            }


            var offset = 0.8f;
            foreach (var card in deck)
            {
                card.transform.localPosition = new Vector3(0f, offset, 0f);
                card.transform.eulerAngles = new Vector3(0, 0, 180f);
                offset += 0.8f;
            }

            Debug.Log("Shuffled");
        }

        public List<CardView> GetFullDeck(EPlayers player)
        {
            if (_random == null)
            {
                _random = new Random();
            }
            
            if (!IsDecksBuilded())
            {
                if (_playerDeck1 == null)
                {
                    _playerDeck1 = BuildDeck(EPlayers.FirstPlayer);
                }

                if (_playerDeck2 == null)
                {
                    _playerDeck2 = BuildDeck(EPlayers.SecondPlayer);
                }
            }

            return player == EPlayers.FirstPlayer ? _playerDeck1 : _playerDeck2;
        }

        public CardView GetTopCardFromDeck(Player player)
        {
            var _currentDeck = player.MyCardsInDeck;
            var _topCard = _currentDeck[_currentDeck.Count - 1];
            _currentDeck.Remove(_topCard);
            return _topCard;
        }

        public CardView SummonCard(int id, Transform slot)
        {
            var createdCard = _cardFactory.SummonCardForAbility(id, slot);
            createdCard.transform.localPosition = new Vector3(0f, 0, 0f);
            createdCard.transform.eulerAngles = new Vector3(0, 0, 180f);
            return createdCard;
        }
    }
}