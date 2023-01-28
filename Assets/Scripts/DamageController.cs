using UnityEngine;

namespace DefaultNamespace
{
    public class DamageController : ICardClickListener, IPlayerClickListener
    {
        public IDamageable DamageDealer;
        public IDamageable AttackedTarget;
        
        
        

        private void Attack()
        {
           var damage =  DamageDealer.GetDamage();
           AttackedTarget.ApplyDamage(damage);

           damage = AttackedTarget.GetDamage();
           DamageDealer.ApplyDamage(damage);
        }

        public void OnCardClick(CardClickSignal signal)
        {
            
        }

        public void OnPlayerClick(PlayerClickSignal signal)
        {
           Debug.Log($"{signal.PlayerView.PlayerType} имеет {signal.PlayerView.GetCurrentMana()} маны и {signal.PlayerView.GetHealth()} HP.");
        }
    }
}