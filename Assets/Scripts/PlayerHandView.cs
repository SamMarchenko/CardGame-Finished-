using System.Collections;
using Cards;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class PlayerHandView : MonoBehaviour
    {
        private Transform[] _handPositions;
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
                Destroy(cardView.gameObject);
                return;
            }

            _cards[result] = cardView;
            StartCoroutine(MoveInHand(cardView,_handPositions[result]));
        }

        private IEnumerator MoveInHand(CardView cardView, Transform parent)
        {
            var time = 0f;
            var startPos = cardView.transform.position;
            var endPos = parent.position;
            cardView.SwitchVisual();
            cardView.transform.eulerAngles = new Vector3(0, 0, 180);
            while (time < 1f)
            {
                cardView.transform.position = Vector3.Lerp(startPos, endPos, time);
                time += Time.deltaTime;
                yield return null;
            }

            cardView.transform.parent = parent;
            cardView.StateType = ECardStateType.InHand;
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
}