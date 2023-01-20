using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class PlayerHandView : MonoBehaviour
    {
        private Transform[] _handPositions;
        //todo: переделать на лист
        private CardView[] _cards;
        
        public void Init(Transform[] handPositions)
        {
            _handPositions = handPositions;
            _cards = new CardView[_handPositions.Length];
        }
        

        public void SetNewCard(CardView cardView)
        {
            var result = GetLastPosition();
            if (result == -1)
            {
                cardView.DestroySelf();
                return;
            }

            _cards[result] = cardView;
            cardView.MoveAnimation(_handPositions[result]);
        }
        
        private int GetLastPosition()
        {
            for (int i = 0; i < _cards.Length; i++)
            {
                if (_cards[i] == null) return i;
            }

            return -1;
        }
    }

    public class PlayerDeckView
    {
        private Transform[] _deckPositions;
        private List<CardView> _cards;
        
        public void Init(Transform[] deckPositions)
        {
            _deckPositions = deckPositions;
            _cards = new List<CardView>(deckPositions.Length);
        }
    }
}