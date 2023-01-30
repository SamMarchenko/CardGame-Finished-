using Cards;
using UnityEngine;

namespace DefaultNamespace
{
    public class DamageController : ICardClickListener, IPlayerClickListener, IChangeStageListener,
        IChangeCurrentPlayerListener
    {
        private ECurrentStageType _currentStageType;
        private Player _currentPlayer;
        private EPlayers _currentPlayerType;
        public IDamageable DamageDealer;
        public IDamageable AttackedTarget;


        private void Attack()
        {
            var damage = DamageDealer.GetDamage();
            AttackedTarget.ApplyDamage(damage);

            damage = AttackedTarget.GetDamage();
            DamageDealer.ApplyDamage(damage);
            DamageDealer.SetCoolDownAttack(true);
        }

        private void ClearDealerAndTarget()
        {
            DamageDealer = null;
            AttackedTarget = null;
        }

        public void OnCardClick(CardClickSignal signal)
        {
            if (_currentStageType != ECurrentStageType.MoveStage)
            {
                return;
            }

            if (signal.CardView.Owner != _currentPlayer && DamageDealer == null)
            {
                return;
            }

            if (signal.CardView.Owner != _currentPlayer)
            {
                AttackedTarget = signal.CardView;
                Attack();
                ClearDealerAndTarget();
            }

            if (signal.CardView.CanAttack)
            {
                DamageDealer = signal.CardView;
            }
        }

        public void OnPlayerClick(PlayerClickSignal signal)
        {
            if (_currentStageType != ECurrentStageType.MoveStage)
            {
                return;
            }

            if (DamageDealer != null)
            {
                AttackedTarget = signal.PlayerView;
                Attack();
                ClearDealerAndTarget();
            }

            Debug.Log($"{signal.PlayerView.PlayerType} имеет {signal.PlayerView.GetCurrentMana()}"
                      + $" маны и {signal.PlayerView.GetHealth()} HP.");
        }

        public void OnStageChange(StageChangeSignal signal)
        {
            _currentStageType = signal.StageType;
        }

        public void OnCurrentPlayerChange(CurrentPlayerChangeSignal signal)
        {
            _currentPlayerType = signal.PlayerType;
            _currentPlayer = signal.Player;
        }
    }
}