using System;
using System.Collections.Generic;
using Cards;
using Signals;
using UnityEngine;

namespace DefaultNamespace
{
    public class Player
    {
        private PlayerView _playerView;
        public PlayerView PlayerView => _playerView;
        private PlayerSignalBus _playerSignalBus;

        private Transform _myDeckSlot;
        private Transform _enemyDeckSlot;
        private List<CardView> _myCardsInDeck;
        public List<CardView> MyCardsInDeck => _myCardsInDeck;

        private Transform[] _myHandSlots;
        private Transform[] _enemyHandSlots;
        private List<CardView> _myCardsInHand;
        
        public List<CardView> MyIncreaseStatsCardsOnTable = new List<CardView>();
        public List<CardView> MyCardsInHand => _myCardsInHand;
        private Dictionary<CardView, Transform> _handCardSlotDictionary = new Dictionary<CardView, Transform>();
        private List<Transform> _canSwapedCardSlots = new List<Transform>(3);

        private Transform[] _myTableSlots;
        private Transform[] _enemyTableSlots;
        private List<CardView> _myCardsInTable;
        public List<CardView> MyCardsInTable => _myCardsInTable;
        private Dictionary<CardView, Transform> _tableCardSlotDictionary = new Dictionary<CardView, Transform>();

        private DeckBuilder _deckBuilder;

        private int _startHandCards = 3;
        private ECurrentStageType _currentStageType;


        private bool _firstMove = true;


        public EPlayers PlayerType;
        private AbilitiesProvider _abilitiesProvider;

        public void SetCurrentStageType(ECurrentStageType stageType)
        {
            _currentStageType = stageType;
        }

        public int GetCurrentMana()
        {
            return _playerView.GetCurrentMana();
        }

        public bool IsEnoughMana(CardView card)
        {
            return _playerView.GetCurrentMana() - card.GetCost() >= 0;
        }

        public void Init(ParentView parentView, EPlayers player, PlayerView playerView, PlayerSignalBus playerSignalBus,
            AbilitiesProvider abilitiesProvider)
        {
            PlayerType = player;
            _playerSignalBus = playerSignalBus;
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

            _abilitiesProvider = abilitiesProvider;
            _playerView = playerView;
            _playerView.Init(PlayerType, _playerSignalBus);
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
            
            ActivateAllAbilities(card);

            //todo: пока абилка "Charge" включается тут при выкладывании карты на стол
            // if (card.MyAbilities.Contains(EAbility.Charge))
            // {
            //     card.CanAttack = true;
            // }


            return slot;
        }

        private void ActivateAllAbilities(CardView card)
        {
            _abilitiesProvider.ActivateAbilities(card);
        }

        public void SetCardFromHandInDeck(CardView card)
        {
            var state = ECardStateType.InDeck;

            _myCardsInHand.Remove(card);
            _myCardsInDeck.Add(card);

            var slot = _handCardSlotDictionary[card];
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
            _abilitiesProvider.DeactivateAbilities(card);
            DeleteCardFromDictionary(_tableCardSlotDictionary, card);
            card.DestroySelf();
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
            CanAttacksFromTable();
            _firstMove = false;
            _playerView.ManaLog();
        }

        private void CanDragCardsFromHand()
        {
            foreach (var card in _myCardsInHand)
            {
                card.CanBeDragged = true;
            }
        }

        private void CanAttacksFromTable()
        {
            foreach (var card in _myCardsInTable)
            {
                card.CanAttack = true;
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
            _playerView.SetCurrentMana(_playerView.GetMaxMana());
        }

        private void ManaIncrease()
        {
            var maxMana = _playerView.GetMaxMana();
            if (maxMana < 10)
            {
                maxMana++;
                _playerView.SetMaxMana(maxMana);
                return;
            }

            _playerView.SetMaxMana(10);
        }

        public void ManaUse(CardView card)
        {
            _playerView.ManaUse(card);
        }

        public bool HasTauntOnTable()
        {
            foreach (var card in _myCardsInTable)
            {
                if (card.IsTaunt)
                {
                    return true;
                }
            }

            return false;
        }
    }
}