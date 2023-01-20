using System.Collections.Generic;
using Cards;
using UnityEngine;

namespace DefaultNamespace
{
    public class Player
    {
        private Transform _myDeckPosition;
        private Transform _enemyDeckPosition;
        

        private Transform[] _myHandPositions;
        private Transform[] _enemyHandPositions;
        private List<CardView> _myCardsInHand;
        
        private Transform[] _myTablePositions;
        private Transform[] _enemyTablePositions;
        private List<CardView> _myCardsInTable;
        
        public void Init(ParentView parentView, EPlayers player)
        {
            if (player == EPlayers.FirstPlayer)
            {
                _myDeckPosition = parentView.Deck1Parent;
                _enemyDeckPosition = parentView.Deck2Parent;
                _myHandPositions = parentView.Hand1Parent;
                _enemyHandPositions = parentView.Hand2Parent;
                _myTablePositions = parentView.Table1Parent;
                _enemyTablePositions = parentView.Table2Parent;
            }
            else
            {
                _myDeckPosition = parentView.Deck2Parent;
                _enemyDeckPosition = parentView.Deck1Parent;
                _myHandPositions = parentView.Hand2Parent;
                _enemyHandPositions = parentView.Hand1Parent;
                _myTablePositions = parentView.Table2Parent;
                _enemyTablePositions = parentView.Table1Parent; 
            }
        }


    }
}