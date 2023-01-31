using System.Collections.Generic;
using Cards;
using DefaultNamespace;
using OneLine;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "NewCardBuffParameters", menuName = "CardIncreaseStats/CardIncreaseStats Parameters")]
    public class CardIncreaseStatsParameters : ScriptableObject
    {
        [SerializeField] private CardPacksContainer _cardPacksContainer;

        [SerializeField, OneLine(Header = LineHeader.Short)]
        private List<BuffParameters> _increaseStatsConfig;
        private List<BuffParameters> _savedStatsConfig;
        public List<BuffParameters> IncreaseStatsConfig => _increaseStatsConfig;
        
        [ContextMenu("Set all cards with ability \"Increase Stats\"" )]
        public void SetCardsWithIncreaseStats()
        {
            _increaseStatsConfig.Clear();
            var IdCardsWithBuffs = _cardPacksContainer.GetCardsWithAbilityType(EAbility.IncreaseStats);
            _increaseStatsConfig.Capacity = IdCardsWithBuffs.Count;
            
            for (var i = 0; i < _increaseStatsConfig.Capacity; i++)
            {
                _increaseStatsConfig.Add(new BuffParameters());
                _increaseStatsConfig[i].ID = IdCardsWithBuffs[i];
            }
        }

        [ContextMenu("Save list \"Increase Stats\". Перезатрёт текущее сохранение")]
        public void SaveIncreaseStatsList()
        {
            _savedStatsConfig = new List<BuffParameters>(_increaseStatsConfig.Count);
            for (var i = 0; i < _savedStatsConfig.Capacity; i++)
            {
                _savedStatsConfig.Add(new BuffParameters());
                _savedStatsConfig[i].DamageBuff = _increaseStatsConfig[i].DamageBuff;
                _savedStatsConfig[i].HpBuff = _increaseStatsConfig[i].HpBuff;
                _savedStatsConfig[i].ID = _increaseStatsConfig[i].ID;
                _savedStatsConfig[i].UnitTypeBuff = _increaseStatsConfig[i].UnitTypeBuff;
            }
        }
        
        [ContextMenu("Load list \"Increase Stats\"")]
        public void LoadIncreaseStatsList()
        {
            _increaseStatsConfig.Clear();
            _increaseStatsConfig.Capacity = _savedStatsConfig.Count;
            for (var i = 0; i < _increaseStatsConfig.Capacity; i++)
            {
                _increaseStatsConfig.Add(new BuffParameters());
                _increaseStatsConfig[i].DamageBuff = _savedStatsConfig[i].DamageBuff;
                _increaseStatsConfig[i].HpBuff = _savedStatsConfig[i].HpBuff;
                _increaseStatsConfig[i].ID = _savedStatsConfig[i].ID;
                _increaseStatsConfig[i].UnitTypeBuff = _savedStatsConfig[i].UnitTypeBuff;
            }
        }
        
        
    }
}