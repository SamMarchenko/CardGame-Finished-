using UnityEngine;

namespace DefaultNamespace
{
    public class AnimationController
    {
        public void PlayDamageAnimation(CardView card)
        {
           var animator = card.gameObject.GetComponent<Animator>();
           animator.SetTrigger("Damage");
        }

        public void PlayDeathAnimation(CardView card)
        {
            var animator = card.GetComponent<Animator>();
            animator.SetTrigger("Death");
            // в конце анимации смерти стоит ивент дестроя карты
        }

        public void PlaySummonCardAnimation(CardView card)
        {
            var animator = card.gameObject.GetComponent<Animator>();
            animator.SetTrigger("Summon");
        }

        public void PlayRollCardAnimation(CardView card)
        {
            var animator = card.gameObject.GetComponent<Animator>();
            animator.SetTrigger("Roll");
        }
    }
}