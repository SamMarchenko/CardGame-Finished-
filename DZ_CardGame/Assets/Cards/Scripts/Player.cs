using UnityEngine;

namespace Cards
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private ESideType _type;
        [SerializeField] private int _health = 30;
        [SerializeField] private int _maxManaPool = 3;
        [SerializeField] private int _currentManaPool = 3;
        public int CurrentManaPool => _currentManaPool;
        public ESideType Type => _type;
        public void IncreaseMaxManaPool()
        {
            if (_maxManaPool == 10) return;
            _maxManaPool++;
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