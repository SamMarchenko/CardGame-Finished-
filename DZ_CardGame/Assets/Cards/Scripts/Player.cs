using UnityEngine;

namespace Cards
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private ESideType _type;
        [SerializeField] private int _health = 30;
        [SerializeField] private int _maxManaPool = 3;
        [SerializeField] private int _currentManaPool = 3;
        public bool IsStartHandCreated;
        private PlayerHand _hand;
        public PlayerHand Hand => _hand;
        private PlayerTable _table;
        public PlayerTable Table => _table;
        private PlayerDeck _deck;
        public ETurn Turn;
        public int CurrentManaPool => _currentManaPool;
        public ESideType Type => _type;
        public void IncreaseMaxManaPool()
        {
            if (_maxManaPool == 10) return;
            _maxManaPool++;
        }

        public void Init(PlayerHand hand, PlayerTable table, PlayerDeck deck)
        {
            _hand = hand;
            _table = table;
            _deck = deck;
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