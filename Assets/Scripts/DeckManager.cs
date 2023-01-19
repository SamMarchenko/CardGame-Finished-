using System.Collections.Generic;
using Cards;
using Zenject;

namespace DefaultNamespace
{
    public class DeckManager : IInitializable
    {
        private readonly DeckFactory _deckFactory;
        private List<CardView> _playerDeck1;
        private List<CardView> _playerDeck2;
        


        public DeckManager(DeckFactory deckFactory)
        {
            _deckFactory = deckFactory;
        }

        public void Initialize()
        {
            _playerDeck1 = _deckFactory.CreateDeck(EPlayers.FirstPlayer);
            _playerDeck2 = _deckFactory.CreateDeck(EPlayers.SecondPlayer);
        }

        public List<CardView> GetFullDeck(EPlayers player)
        {
            return player == EPlayers.FirstPlayer ? _playerDeck1 : _playerDeck2;
        }

        public CardView GetTopCardFromDeck(EPlayers player)
        {
            var _currentDeck = GetFullDeck(player);
            return _currentDeck[_currentDeck.Count];
        }
        
    }
}