using Cards;
using UnityEngine;

namespace DefaultNamespace
{
    public class DamageController : ICardClickListener, IPlayerClickListener, IChangeStageListener,
        IChangeCurrentPlayerListener
    {
        private readonly PlayersProvider _playersProvider;
        private ECurrentStageType _currentStageType;
        private Player _currentDamageDealerPlayer;
        private Player _attackedPlayer;
        private EPlayers _currentPlayerType;
        public IDamageable DamageDealer;
        public IDamageable AttackedTarget;

        public DamageController(PlayersProvider playersProvider)
        {
            _playersProvider = playersProvider;
        }


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

            if (signal.CardView.Owner != _currentDamageDealerPlayer && DamageDealer == null)
            {
                return;
            }

            if (signal.CardView.Owner != _currentDamageDealerPlayer && signal.CardView.StateType == ECardStateType.OnTable)
            {
                if (signal.CardView.Owner.HasTauntOnTable() && !signal.CardView.MyAbilities.Contains(EAbility.Taunt))
                {
                    Debug.Log("У героя на столе есть Taunt. Атаковать других юнитов запрещено!!!");
                    return;
                }
                AttackedTarget = signal.CardView;
                Attack();
                ClearDealerAndTarget();
                return;
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
                var attackedPlayer = _playersProvider.GetPlayer(signal.PlayerView.PlayerType);
                if (attackedPlayer.HasTauntOnTable())
                {
                    Debug.Log("У данного героя есть Taunt на столе!!!");
                    return;
                }
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
            _currentDamageDealerPlayer = signal.Player;
            
            _attackedPlayer = _playersProvider.GetPlayer(_currentPlayerType == EPlayers.FirstPlayer ? EPlayers.SecondPlayer : EPlayers.FirstPlayer);
        }
    }
}