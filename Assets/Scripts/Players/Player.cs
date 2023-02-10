using System;
using System.Collections.Generic;
using Cards;
using Signals;
using UnityEngine;

namespace DefaultNamespace
{
    public class Player
    {
        private Transform _myDeckSlot;
        private Transform _enemyDeckSlot;
        private Transform[] _myHandSlots;
        private Transform[] _enemyHandSlots;
        private Transform[] _myTableSlots;
        private Transform[] _enemyTableSlots;
        private List<CardView> _myCardsInDeck;
        private List<CardView> _myCardsInHand;
        private List<Transform> _canSwapedCardSlots = new List<Transform>(3);
        private List<CardView> _myCardsInTable;
        private Dictionary<CardView, Transform> _handCardSlotDictionary = new Dictionary<CardView, Transform>();
        private Dictionary<CardView, Transform> _tableCardSlotDictionary = new Dictionary<CardView, Transform>();

        private PlayerView _playerView;
        private PlayerSignalBus _playerSignalBus;
        private DeckBuilder _deckBuilder;
        private AbilitiesProvider _abilitiesProvider;
        private BuffController _buffController;

        private int _startHandCards = 3;
        private ECurrentStageType _currentStageType;
        private bool _firstMove = true;


        public PlayerView PlayerView => _playerView;
        public List<CardView> MyCardsInDeck => _myCardsInDeck;
        public List<CardView> MyCardsInHand => _myCardsInHand;
        public List<CardView> MyCardsInTable => _myCardsInTable;
        public List<CardView> MyBuffersInTable = new List<CardView>();
        public EPlayers PlayerType;
        private AnimationController _animationController;

        public void Init(ParentView parentView, EPlayers player, PlayerView playerView, PlayerSignalBus playerSignalBus,
            AbilitiesProvider abilitiesProvider, BuffController buffController, AnimationController animationController )
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

            _animationController = animationController;
            _abilitiesProvider = abilitiesProvider;
            _buffController = buffController;
            _playerView = playerView;
            _playerView.Init(PlayerType, _playerSignalBus);
            _myCardsInDeck = new List<CardView>();
            _myCardsInHand = new List<CardView>(_myHandSlots.Length);
            _myCardsInTable = new List<CardView>(_myTableSlots.Length);
            SetFirstThreeHandSlots();
        }

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

        public Transform SetCardFromDeckInHand()
        {
            var card = _deckBuilder.GetTopCardFromDeck(this);
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


        public void StartOfMove()
        {
            if (!_firstMove)
            {
                //var card = _deckBuilder.GetTopCardFromDeck(this);
                // SetCardFromDeckInHand(card);
                SetCardFromDeckInHand();
                ManaIncrease();
                ManaRegenerate();
            }

            CanDragCardsFromHand();
            CanAttacksFromTable();
            _firstMove = false;
            _playerView.ManaLog();
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
                _animationController.PlayRollCardAnimation(card);
                card.IsEnable = visible;
            }
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

            _abilitiesProvider.ActivateAbilitiesByThisCard(card);
            _buffController.CheckAndGiveBuffToThisCard(card);

            return slot;
        }

        public void SummonCardOnTableFromBattlecry(int id, int dmg, int hp, Transform slot)
        {
            var card = _deckBuilder.SummonCard(id, slot);
            card.SetDamage(dmg, 0);
            card.SetHealth(hp, 0);
            card.Owner = this;
            AddCardInDictionary(_tableCardSlotDictionary, card, slot, ECardStateType.OnTable);
            _myCardsInTable.Add(card);

            _abilitiesProvider.ActivateAbilitiesByThisCard(card);
            _buffController.CheckAndGiveBuffToThisCard(card);
            _animationController.PlaySummonCardAnimation(card);
        }

        public void KillCardFromTable(CardView card)
        {
            _animationController.PlayDeathAnimation(card);
            _myCardsInTable.Remove(card);
            DeleteCardFromDictionary(_tableCardSlotDictionary, card);

            _abilitiesProvider.DeactivateAbilitiesByThisCard(card);
            
            //card.DestroySelf();
        }


        private void SetDeck()
        {
            _myCardsInDeck = _deckBuilder.GetFullDeck(PlayerType);
            foreach (var card in _myCardsInDeck)
            {
                card.Owner = this;
            }
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

        public Transform GiveFirstFreeTableSlotForDrawAbility()
        {
            var slot = FindFirstFreeSlot(_myTableSlots, _tableCardSlotDictionary);
            return slot;
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

        public void PlayDamageAnimation(CardView card)
        {
            _animationController.PlayDamageAnimation(card);
        }
    }
}