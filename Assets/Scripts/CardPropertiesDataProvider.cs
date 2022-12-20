using System.Collections.Generic;
using Cards;
using Zenject;

namespace DefaultNamespace
{
    public class CardPropertiesDataProvider : IInitializable
    {
        private List<CardPropertiesData> _allCards = new List<CardPropertiesData>();
        private readonly CardPacksContainer _packsContainer;

        public CardPropertiesDataProvider(CardPacksContainer packsContainer)
        {
            _packsContainer = packsContainer;
        }

        public void Initialize()
        {
            FillCards();
        }
        
        public List<CardPropertiesData> GetAllCards()
        {
            return _allCards;
        }

        private void FillCards()
        {
            foreach (var item in _packsContainer.CardPackConfigurations)
            {
                _allCards.AddRange(item.Cards);
            }
        }

        
    }
}