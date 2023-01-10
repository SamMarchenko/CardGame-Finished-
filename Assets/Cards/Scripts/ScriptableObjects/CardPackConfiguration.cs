using OneLine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cards.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewCardPackConfiguration", menuName = "CardConfigs/Card Pack Configuration")]
    public class CardPackConfiguration : ScriptableObject
    {
        [SerializeField] private EHeroType heroType;
        public EHeroType HeroType => heroType;

        [SerializeField, OneLine(Header = LineHeader.Short)]
        private CardPropertiesData[] _cards;

        public CardPropertiesData[] Cards => _cards;

        public IEnumerable<CardPropertiesData> UnionProperties(IEnumerable<CardPropertiesData> array)
        {
            return array.Union(_cards);
        }
    }
}