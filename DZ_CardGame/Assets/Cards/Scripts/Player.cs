using System;
using UnityEngine;

namespace Cards
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private ESideType _type;
        [SerializeField] private int _health = 30;
        [SerializeField] private int _maxManaPool = 3;
        [SerializeField] private int _currentManaPool = 3;
        private HandSlotsHandler _handSlotsHandler;
        public EGameStage Stage { get; set; }
        [SerializeField] private PlayerHand _hand;
        public PlayerHand Hand => _hand;
        
        [SerializeField] private PlayerTable _table;
        public PlayerTable Table => _table;
        
        [SerializeField] private PlayerDeck _deck;
        public PlayerDeck Deck => _deck;
        public ETurn Turn;
        public int CurrentManaPool => _currentManaPool;
        public ESideType Type => _type;
        public void IncreaseMaxManaPool()
        {
            if (_maxManaPool == 10) return;
            _maxManaPool++;
        }

        public void Init(HandSlotsHandler handSlotsHandler)
        {
            _handSlotsHandler = handSlotsHandler;
        }

        private void Start()
        {
            _hand.Init(_handSlotsHandler);
        }

        public void RecoverManaPool()
        {
            _currentManaPool = _maxManaPool;
        }

        public void SpendManaPool(int cost)
        {
            if (_currentManaPool - cost < 0)
            {
                Debug.LogError("!!!!!Ошибка. Маны не может быть меньше нуля!!!!");
            }
            else
            {
                _currentManaPool -= cost;
            }
        }
    }
}