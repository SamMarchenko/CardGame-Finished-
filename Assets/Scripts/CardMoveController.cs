using UnityEngine;

namespace DefaultNamespace
{
    public class CardMoveController : ICardClickListener
    {
        public void OnCardClick(CardClickSignal signal)
        {
            Debug.Log($"Нажатие на карту {signal.CardView.name}");
        }
    }
}