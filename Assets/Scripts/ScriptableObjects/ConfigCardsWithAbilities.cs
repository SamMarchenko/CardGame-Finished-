using System.Collections.Generic;
using Cards;
using DefaultNamespace;
using OneLine;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "NewConfigCardsWithAbilities", menuName = "CardsWithAbilities")]
    public class ConfigCardsWithAbilities : ScriptableObject
    {
        [SerializeField] private CardPacksContainer _cardPacksContainer;

        [SerializeField, OneLine(Header = LineHeader.Short)]
        private List<IncreaseStatsParameters> _increaseStatsConfig;
        private List<IncreaseStatsParameters> _savedStatsConfig;
        [SerializeField, OneLine(Header = LineHeader.Short)]
        private List<int> _idCardsWithTauntConfig;
        [SerializeField, OneLine(Header = LineHeader.Short)]
        private List<int> _idCardsWithChargeConfig;
        public List<int> IDCardsWithTauntConfig => _idCardsWithTauntConfig;
        public List<int> IDCardsWithChargeConfig => _idCardsWithChargeConfig;
        public List<IncreaseStatsParameters> IncreaseStatsConfig => _increaseStatsConfig;

        [ContextMenu("Set all cards with ability \"Taunt\"")]
        public void SetCardsWithTaunt()
        {
            _idCardsWithTauntConfig.Clear();
            var list = _cardPacksContainer.GetCardsWithAbilityType(EAbility.Taunt);
            _idCardsWithTauntConfig.Capacity = list.Count;
            for (var i = 0; i < _idCardsWithTauntConfig.Capacity; i++)
            {
                _idCardsWithTauntConfig.Add(list[i]);
            }
        }
        [ContextMenu("Set all cards with ability \"Charge\"")]
        public void SetCardsWithCharge()
        {
            _idCardsWithChargeConfig.Clear();
            var list = _cardPacksContainer.GetCardsWithAbilityType(EAbility.Charge);
            _idCardsWithChargeConfig.Capacity = list.Count;
            for (var i = 0; i < _idCardsWithChargeConfig.Capacity; i++)
            {
                _idCardsWithChargeConfig.Add(list[i]);
            }
        }

        [ContextMenu("Set all cards with ability \"Increase Stats\"" )]
        public void SetCardsWithIncreaseStats()
        {
            _increaseStatsConfig.Clear();
            var IdCardsWithBuffs = _cardPacksContainer.GetCardsWithAbilityType(EAbility.IncreaseStats);
            _increaseStatsConfig.Capacity = IdCardsWithBuffs.Count;
            
            for (var i = 0; i < _increaseStatsConfig.Capacity; i++)
            {
                _increaseStatsConfig.Add(new IncreaseStatsParameters());
                _increaseStatsConfig[i].ID = IdCardsWithBuffs[i];
            }
        }

        [ContextMenu("Save list \"Increase Stats\". Перезатрёт текущее сохранение")]
        public void SaveIncreaseStatsList()
        {
            _savedStatsConfig = new List<IncreaseStatsParameters>(_increaseStatsConfig.Count);
            for (var i = 0; i < _savedStatsConfig.Capacity; i++)
            {
                _savedStatsConfig.Add(new IncreaseStatsParameters());
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
                _increaseStatsConfig.Add(new IncreaseStatsParameters());
                _increaseStatsConfig[i].DamageBuff = _savedStatsConfig[i].DamageBuff;
                _increaseStatsConfig[i].HpBuff = _savedStatsConfig[i].HpBuff;
                _increaseStatsConfig[i].ID = _savedStatsConfig[i].ID;
                _increaseStatsConfig[i].UnitTypeBuff = _savedStatsConfig[i].UnitTypeBuff;
            }
        }
        
        
    }
}