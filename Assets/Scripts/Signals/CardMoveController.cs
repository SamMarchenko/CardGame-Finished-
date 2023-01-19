﻿using System;
using Cards;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class CardMoveController : ICardClickListener, ICardPointerListener, ICardDragListener
    {
        private readonly ParentView _parentView;
        private Camera _camera;

        public CardMoveController(ParentView parentView)
        {
            _parentView = parentView;
            _camera = Camera.main;
        }
        
        public void OnCardClick(CardClickSignal signal)
        {
            Debug.Log($"Нажатие на карту {signal.CardView.name}");
        }

        public void PointerOnCard(CardPointerSignal signal)
        {
            var cardView = signal.CardView;
            switch (cardView.StateType)
            {
                case ECardStateType.InHand:
                    Debug.Log($"Курсор наведен на карту {signal.CardView.NameText.text}");
                    ScaleCard(EScaleType.Increase, cardView);
                    break;
                case ECardStateType.OnTable:
                    break;
            }
        }

        public void PointerOffCard(CardPointerSignal signal)
        {
            var cardView = signal.CardView;

            switch (cardView.StateType)
            {
                case ECardStateType.InHand:
                    Debug.Log($"Курсор убран с карты {signal.CardView.NameText.text}");
                    ScaleCard(EScaleType.Decrease, cardView);
                    break;
                case ECardStateType.OnTable:
                    break;
            }
        }

        


        public void OnDragCardStart(CardDragSignal signal)
        {
            ScaleCard(EScaleType.Increase, signal.CardView);
        }

        public void OnDraggingCard(CardDragSignal signal)
        {
            var cardView = signal.CardView;
            DragCard(cardView);
        }

        public void OnDragCardEnd(CardDragSignal signal)
        {
            ScaleCard(EScaleType.Decrease, signal.CardView);
        }
        
        
        
        private void ScaleCard(EScaleType scaleType, CardView cardView)
        {
            switch (scaleType)
            {
                case EScaleType.Increase:
                    cardView.transform.localPosition += cardView.StepPosition;
                    cardView.transform.localScale *= cardView.Scale;
                    break;
                case EScaleType.Decrease:
                    cardView.transform.localPosition -= cardView.StepPosition;
                    cardView.transform.localScale /= cardView.Scale;
                    break;
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
    }
}