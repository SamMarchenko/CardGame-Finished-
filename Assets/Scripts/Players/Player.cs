using System.Collections.Generic;
using Cards;
using UnityEngine;

namespace DefaultNamespace
{
    public class Player
    {
        private Transform _myDeckSlot;
        private Transform _enemyDeckSlot;
        private List<CardView> _myCardsInDeck;
        public List<CardView> MyCardsInDeck => _myCardsInDeck;

        private Transform[] _myHandSlots;
        private Transform[] _enemyHandSlots;
        private List<CardView> _myCardsInHand;
        private Dictionary<CardView, Transform> _handCardSlotDictionary = new Dictionary<CardView, Transform>();

        private Transform[] _myTableSlots;
        private Transform[] _enemyTableSlots;
        private List<CardView> _myCardsInTable;
        private Dictionary<CardView, Transform> _tableCardSlotDictionary = new Dictionary<CardView, Transform>();

        private DeckBuilder _deckBuilder;

        public bool ChooseStartHand = false;
        public EPlayers PlayerType;

        public void Init(ParentView parentView, EPlayers player)
        {
            PlayerType = player;
            if (player == EPlayers.FirstPlayer)
            {
                _myDeckSlot = parentView.Deck1Parent;
                _enemyDeckSlot = parentView.Deck2Parent;
                _myHandSlots = parentView.Hand1Parent;
                _enemyHandSlots = parentView.Hand2Parent;
                _myTableSlots = parentView.Table1Parent;
                _enemyTableSlots = parentView.Table2Parent;
            }
            else
            {
                _myDeckSlot = parentView.Deck2Parent;
                _enemyDeckSlot = parentView.Deck1Parent;
                _myHandSlots = parentView.Hand2Parent;
                _enemyHandSlots = parentView.Hand1Parent;
                _myTableSlots = parentView.Table2Parent;
                _enemyTableSlots = parentView.Table1Parent;
            }

            _myCardsInDeck = new List<CardView>();
            _myCardsInHand = new List<CardView>(_myHandSlots.Length);
            _myCardsInTable = new List<CardView>(_myTableSlots.Length);
        }

        public void SetDeckBuilder(DeckBuilder deckBuilder)
        {
            _deckBuilder = deckBuilder;
            SetDeck();
        }

        private void SetDeck()
        {
            _myCardsInDeck = _deckBuilder.GetFullDeck(PlayerType);
            foreach (var card in _myCardsInDeck)
            {
                card.Owner = this;
            }
        }

        public Transform SetCardFromDeckInHand(CardView card)
        {
            var state = ECardStateType.InHand;
            var slot = FindFirstFreeSlot(_myHandSlots, _handCardSlotDictionary);

            if (slot == null)
            {
                Debug.Log("В руке нет свободного слота. Карта из колоды не может добавиться");
                return null;
            }

            _myCardsInDeck.Remove(card);

            AddCardInDictionary(_handCardSlotDictionary, card, slot, state);
            _myCardsInHand.Add(card);
            card.MoveAnimation(slot);

            return slot;
        }

        public Transform SetCardFromHandInTable(CardView card)
        {
            var state = ECardStateType.OnTable;
            var slot = FindFirstFreeSlot(_myTableSlots, _tableCardSlotDictionary);

            if (slot == null)
            {
                Debug.Log("На столе нет свободного слота. Карта из руки не может добавиться");
                return null;
            }

            DeleteCardFromDictionary(_handCardSlotDictionary, card);
            _myCardsInHand.Remove(card);

            AddCardInDictionary(_tableCardSlotDictionary, card, slot, state);
            _myCardsInTable.Add(card);


            return slot;
        }

        public void SetCardFromHandInDeck(CardView card)
        {
            var state = ECardStateType.InDeck;

            _myCardsInHand.Remove(card);
            _myCardsInDeck.Add(card);
            card.StateType = state;
            card.CanSwaped = false;

            card.MoveAnimation(_myDeckSlot);
            _deckBuilder.ShuffleDeck(_myCardsInDeck);
        }

        public void KillCardFromTable(CardView card)
        {
            _myCardsInTable.Remove(card);
            DeleteCardFromDictionary(_tableCardSlotDictionary, card);
            //todo: тут убивается карта. Мб сигнал надо на анимацию оттсюда запускать
        }


        private Transform FindFirstFreeSlot(Transform[] slots, Dictionary<CardView, Transform> dictionary)
        {
            foreach (var slot in slots)
            {
                if (!dictionary.ContainsValue(slot))
                {
                    return slot;
                }
            }

            return null;
        }

        private void AddCardInDictionary(Dictionary<CardView, Transform> dictionary, CardView card, Transform slot,
            ECardStateType state)
        {
            dictionary.Add(card, slot);
            card.StateType = state;
        }

        private void DeleteCardFromDictionary(Dictionary<CardView, Transform> dictionary, CardView card)
        {
            dictionary.Remove(card);
        }
    }
}