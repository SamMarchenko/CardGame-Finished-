using System;
using System.Collections.Generic;
using Cards;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class CardMoverView : ICardClickListener, ICardPointerListener, ICardDragListener, IChangeStageListener,
        IChangeCurrentPlayerListener
    {
        private readonly ParentView _parentView;
        private readonly PlayersProvider _playersProvider;
        private readonly DeckBuilder _deckBuilder;
        private Camera _camera;
        private Player _currentPlayer;
        private ECurrentStageType _currentStageType;
        private float _unwantedDragDistance = 100f;

        public CardMoverView(ParentView parentView, PlayersProvider playersProvider, DeckBuilder deckBuilder)
        {
            _parentView = parentView;
            _playersProvider = playersProvider;
            _deckBuilder = deckBuilder;
            _camera = Camera.main;
        }

        public void OnCardClick(CardClickSignal signal)
        {
            Debug.Log($"Нажатие на карту {signal.CardView.name}");
            switch (_currentStageType)
            {
                case ECurrentStageType.ChooseStartHandStage:
                    OnChooseHandCardClickBehaviour(signal.CardView);
                    break;
                case ECurrentStageType.MoveStage:
                    OnMoveStageCardClickBehaviour(signal.CardView);
                    break;
            }
        }

        private void OnChooseHandCardClickBehaviour(CardView card)
        {
            if (card.StateType == ECardStateType.InDeck)
            {
                Debug.Log("Карта в колоде. Никаких действий с ней не предполагается");
                return;
            }

            if (card.Owner == _currentPlayer && _currentPlayer.CanSwapCard(card))
            {
                _currentPlayer.SetCardFromHandInDeck(card);
              //  _currentPlayer.SetCardFromDeckInHand(_deckBuilder.GetTopCardFromDeck(_currentPlayer));
              _currentPlayer.SetCardFromDeckInHand();
            }
        }

        private void OnMoveStageCardClickBehaviour(CardView card)
        {
        }

        public void PointerOnCard(CardPointerSignal signal)
        {
            var cardView = signal.CardView;
            switch (cardView.StateType)
            {
                case ECardStateType.InHand:
                case ECardStateType.OnTable:
                    cardView.UpScaleCard();
                    break;
            }
        }

        public void PointerOffCard(CardPointerSignal signal)
        {
            var cardView = signal.CardView;

            switch (cardView.StateType)
            {
                case ECardStateType.InHand:
                case ECardStateType.OnTable:
                    cardView.DownScaleCard();
                    break;
            }
        }


        public void OnDragCardStart(CardDragSignal signal)
        {
            // ScaleCard(EScaleType.Increase, signal.CardView);
        }

        public void OnDraggingCard(CardDragSignal signal)
        {
            if (signal.CardView.Owner != _currentPlayer || _currentStageType != ECurrentStageType.MoveStage)
            {
                return;
            }

            var card = signal.CardView;
            
            if (!_currentPlayer.IsEnoughMana(card))
            {
                Debug.Log($"Недостаточно маны. {_currentPlayer.PlayerType} имеет {_currentPlayer.GetCurrentMana()}. Карта стоит {card.GetCost()}");
                return;
            }
            DragCard(card);
        }

        public void OnDragCardEnd(CardDragSignal signal)
        {
            if (_currentStageType != ECurrentStageType.MoveStage || signal.CardView.Owner != _currentPlayer)
            {
                return;
            }
            
            var card = signal.CardView;

            

            switch (card.StateType)
            {
                case ECardStateType.InHand:
                    var startSlot = _currentPlayer.GetCurrentSlotByCard(card);
                    var distance = Vector3.Distance(card.transform.position, startSlot.position);
                    if (distance <= _unwantedDragDistance)
                    {
                        card.MoveAnimation(_currentPlayer.GetCurrentSlotByCard(card));
                        return;
                    }

                    var endSlot = _currentPlayer.SetCardFromHandInTable(card);

                    if (endSlot == null)
                    {
                        card.MoveAnimation(_currentPlayer.GetCurrentSlotByCard(card));
                        return;
                    }

                    card.MoveAnimation(endSlot);
                    _currentPlayer.ManaUse(card);
                    break;
                case ECardStateType.OnTable:
                    //todo: пока если карту тянуть со стола, то она возвращается в свой слот.
                    //Скорее всего надо будет вообще запретить драгать со стола - атаку реализовать через клик по карте своей => карте врага
                    card.MoveAnimation(_currentPlayer.GetCurrentSlotByCard(card));
                    break;
                case ECardStateType.Discard:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void DragCard(CardView cardView)
        {
            Ray R = _camera.ScreenPointToRay(Input.mousePosition);
            Vector3 PO = cardView.transform.position;
            Vector3 PN = -_camera.transform.forward;
            float t = Vector3.Dot(PO - R.origin, PN) / Vector3.Dot(R.direction, PN);
            Vector3 P = R.origin + R.direction * t;

            cardView.transform.position = P;
        }

        public void OnStageChange(StageChangeSignal signal)
        {
            _currentStageType = signal.StageType;
        }

        public void OnCurrentPlayerChange(CurrentPlayerChangeSignal signal)
        {
            _currentPlayer = _playersProvider.GetPlayer(signal.PlayerType);

            //todo: добавить проверку на текущую стадию. Если стадия хода, то текущему игроку восстанавливать и увеличивать ману; брать карту из колоды
        }
    }
}