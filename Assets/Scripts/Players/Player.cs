using System;
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
        private List<Transform> _canSwapedCardSlots = new List<Transform>(3);

        private Transform[] _myTableSlots;
        private Transform[] _enemyTableSlots;
        private List<CardView> _myCardsInTable;
        private Dictionary<CardView, Transform> _tableCardSlotDictionary = new Dictionary<CardView, Transform>();

        private DeckBuilder _deckBuilder;

        private int _startHandCards = 3;
        private ECurrentStageType _currentStageType;

        private int _currentMana = 3;
        private int _maxMana = 3;
        private bool _firstMove = true;
        


        public EPlayers PlayerType;

        public void SetCurrentStageType(ECurrentStageType stageType)
        {
            _currentStageType = stageType;
        }

        public int GetCurrentMana()
        {
            return _currentMana;
        }

        public bool IsEnoughMana(CardView card)
        {
            return _currentMana - card.GetCost() >= 0;
        }

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
            SetFirstThreeHandSlots();
        }

        private void SetFirstThreeHandSlots()
        {
            for (var i = 0; i < _startHandCards; i++)
            {
                _canSwapedCardSlots.Add(_myHandSlots[i]);
            }
        }

        public bool CanSwapCard(CardView card)
        {
            var slot = _handCardSlotDictionary[card];
            
            return _canSwapedCardSlots.Contains(slot);
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
            card.StateType = state;
            card.MoveAnimation(slot);
            card.SwitchVisual();

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
            
            var slot= _handCardSlotDictionary[card];
            DeleteCardFromDictionary(_handCardSlotDictionary, card);
            _canSwapedCardSlots.Remove(slot);
            
            card.StateType = state;
            card.MoveAnimation(_myDeckSlot);
            card.SwitchVisual();
            _deckBuilder.ShuffleDeck(_myCardsInDeck);
        }

        public void KillCardFromTable(CardView card)
        {
            _myCardsInTable.Remove(card);
            DeleteCardFromDictionary(_tableCardSlotDictionary, card);
            //todo: тут убивается карта. Мб сигнал надо на анимацию оттсюда запускать
        }
        
        public void StartOfMove()
        {
            
            if (!_firstMove)
            {
                var card = _deckBuilder.GetTopCardFromDeck(this);
                SetCardFromDeckInHand(card);
                ManaIncrease();
                ManaRegenerate();
            }
            
            CanDragCardsFromHand();
            _firstMove = false;
            Debug.Log($"У игрока {PlayerType} {_currentMana}/{_maxMana} маны");
        }

        private void CanDragCardsFromHand()
        {
            foreach (var card in _myCardsInHand)
            {
                card.CanBeDragged = true;
            }
        }


        public Transform GetCurrentSlotByCard(CardView card)
        {
            switch (card.StateType)
            {
                case ECardStateType.InDeck:
                    return _myDeckSlot;
                case ECardStateType.InHand:
                    return _handCardSlotDictionary[card];
                case ECardStateType.OnTable:
                    return _tableCardSlotDictionary[card];
            }

            return null;
        }

        public void RollCardsInHand(bool visible)
        {
            foreach (var card in _myCardsInHand)
            {
                card.IsEnable = visible;
            }
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

        private void ManaRegenerate()
        {
            _currentMana = _maxMana;
        }

        private void ManaIncrease()
        {
            if (_maxMana < 10)
            {
                _maxMana++;
                return;
            }
            _maxMana = 10;
        }

        public void ManaUse(CardView card)
        {
            _currentMana -= card.GetCost();
            Debug.Log($"У игрока {PlayerType} осталось {_currentMana}/{_maxMana} маны.");
        }
        
        


    }
}