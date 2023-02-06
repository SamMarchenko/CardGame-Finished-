using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Cards;
using DefaultNamespace;
using OneLine;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "NewConfigCardsWithAbilities", menuName = "CardsWithAbilities")]
    public class ConfigCardsWithAbilities : ScriptableObject
    {

        [SerializeField] private CardPacksContainer _cardPacksContainer;

        [SerializeField, OneLine(Header = LineHeader.Short)]
        private List<BuffParameters> _increaseStatsConfig;
        private List<BuffParameters> _savedStatsConfig;
        [SerializeField, OneLine(Header = LineHeader.Short)]
        private List<int> _idCardsWithTauntConfig;
        [SerializeField, OneLine(Header = LineHeader.Short)]
        private List<int> _idCardsWithChargeConfig;
        [SerializeField, OneLine(Header = LineHeader.Short)]
        private List<BattlecryParameters> _battlecriesConfig;

        private List<BattlecryParameters> _savedBattlecriesList;

        public List<BattlecryParameters> BattlecriesConfig => _battlecriesConfig;
        public List<int> IDCardsWithTauntConfig => _idCardsWithTauntConfig;
        public List<int> IDCardsWithChargeConfig => _idCardsWithChargeConfig;
        public List<BuffParameters> IncreaseStatsConfig => _increaseStatsConfig;

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
                _increaseStatsConfig.Add(new BuffParameters());
                _increaseStatsConfig[i].ID = IdCardsWithBuffs[i];
            }
        }
        [ContextMenu("Set all cards with ability \"BattleCry\"" )]
        public void SetCardsWithBattleCry()
        {
            _battlecriesConfig.Clear();
            var list = _cardPacksContainer.GetCardsWithAbilityType(EAbility.Battlecry);
            _battlecriesConfig.Capacity = list.Count;
            
            for (var i = 0; i < _battlecriesConfig.Capacity; i++)
            {
                _battlecriesConfig.Add(new BattlecryParameters());
                _battlecriesConfig[i].ID = list[i];
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
        
        [ContextMenu("Save list \"Battlecries\". Перезатрёт текущее сохранение")]
        public void SaveBattlecryList()
        {
            _savedBattlecriesList = new List<BattlecryParameters>(_battlecriesConfig.Count);
            for (var i = 0; i < _savedBattlecriesList.Capacity; i++)
            {
                _savedBattlecriesList.Add(new BattlecryParameters());
                _savedBattlecriesList[i].ID = _battlecriesConfig[i].ID;
                _savedBattlecriesList[i].Action = _battlecriesConfig[i].Action;
                _savedBattlecriesList[i].DMG = _battlecriesConfig[i].DMG;
                _savedBattlecriesList[i].HP = _battlecriesConfig[i].HP;
                _savedBattlecriesList[i].SummonId = _battlecriesConfig[i].SummonId;
                _savedBattlecriesList[i].Target = _battlecriesConfig[i].Target;
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
        
        [ContextMenu("Load list \"Battlecies\"")]
        public void LoadBattlecriesList()
        {
            _battlecriesConfig.Clear();
            _battlecriesConfig.Capacity = _savedBattlecriesList.Count;
            for (var i = 0; i < _battlecriesConfig.Capacity; i++)
            {
                _battlecriesConfig.Add(new BattlecryParameters());
                _battlecriesConfig[i].ID = _savedBattlecriesList[i].ID;
                _battlecriesConfig[i].Action = _savedBattlecriesList[i].Action;
                _battlecriesConfig[i].DMG = _savedBattlecriesList[i].DMG;
                _battlecriesConfig[i].HP = _savedBattlecriesList[i].HP;
                _battlecriesConfig[i].SummonId = _savedBattlecriesList[i].SummonId;
                _battlecriesConfig[i].Target = _savedBattlecriesList[i].Target;
            }
        }
        
        
    }
}