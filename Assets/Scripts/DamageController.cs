using System;
using Cards;
using Signals;
using UnityEngine;

namespace DefaultNamespace
{
    public class DamageController : ICardClickListener, IPlayerClickListener, IChangeStageListener,
        IChangeCurrentPlayerListener, ICardDoBattlecryListener, ICardBattlecryAttackListener
    {
        private readonly PlayersProvider _playersProvider;
        private readonly CardSignalBus _cardSignalBus;
        private ECurrentStageType _currentStageType;
        private EBattlecrySubStage _battlecrySubStage;
        private Player _currentDamageDealerPlayer;
        private Player _attackedPlayer;
        private EPlayers _currentPlayerType;
        private int _battlecryDamage;
        private bool _isPointBattlecryTarget;


        public EBattlecryTarget CurrentBattlecryTarget = EBattlecryTarget.None;
        public IDamageable DamageDealer;
        public IDamageable AttackedTarget;

        public DamageController(PlayersProvider playersProvider, CardSignalBus cardSignalBus)
        {
            _playersProvider = playersProvider;
            _cardSignalBus = cardSignalBus;
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

            if (_battlecrySubStage == EBattlecrySubStage.True && _currentPlayerType != signal.CardView.Owner.PlayerType)
            {
                if (!_isPointBattlecryTarget) return;

                var card = signal.CardView;
                card.ApplyDamage(_battlecryDamage);
                _battlecryDamage = 0;
                _cardSignalBus.CardDoBattlecryFire(new CardDoBattlecrySignal(EBattlecrySubStage.False));
            }

            if (_battlecrySubStage == EBattlecrySubStage.True && _currentPlayerType == signal.CardView.Owner.PlayerType)
            {
                Debug.Log("Выберите карту врага на столе, на которую хотите применить Battlecry");
                return;
            }


            if (signal.CardView.Owner != _currentDamageDealerPlayer && DamageDealer == null)
            {
                return;
            }

            if (signal.CardView.Owner != _currentDamageDealerPlayer &&
                signal.CardView.StateType == ECardStateType.OnTable)
            {
                if (signal.CardView.Owner.HasTauntOnTable() && !signal.CardView.IsTaunt)
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

            if (_battlecrySubStage == EBattlecrySubStage.True && _currentPlayerType != signal.PlayerView.PlayerType)
            {
                if (!_isPointBattlecryTarget) return;

                var target = signal.PlayerView;
                target.ApplyDamage(_battlecryDamage);
                
                Debug.Log($"{signal.PlayerView.PlayerType} получил с баттлкрая {_battlecryDamage}" +
                          $" и имеет {signal.PlayerView.GetHealth()} HP.");
                _battlecryDamage = 0;
                
                _cardSignalBus.CardDoBattlecryFire(new CardDoBattlecrySignal(EBattlecrySubStage.False));
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

            _attackedPlayer = _playersProvider.GetPlayer(_currentPlayerType == EPlayers.FirstPlayer
                ? EPlayers.SecondPlayer
                : EPlayers.FirstPlayer);
        }

        public void OnCardDoBattlecry(CardDoBattlecrySignal signal)
        {
            _battlecrySubStage = signal.IsBattleryStage;
        }

        public void OnDoBattlecryAttack(CardBattlecryAttackSignal signal)
        {
            var card = signal.Card;
            _battlecryDamage = card.BattlecryParameters.DMG;
            switch (card.BattlecryParameters.Target)
            {
                case EBattlecryTarget.PointUnit:
                {
                    _isPointBattlecryTarget = true;
                }
                    break;

                case EBattlecryTarget.Hero:
                {
                    _isPointBattlecryTarget = false;
                    var target = card.Owner.PlayerType == EPlayers.FirstPlayer
                        ? _playersProvider.GetPlayer(EPlayers.SecondPlayer)
                        : _playersProvider.GetPlayer(EPlayers.FirstPlayer);
                    target.PlayerView.ApplyDamage(_battlecryDamage);
                    Debug.Log($"{target.PlayerType} получил {card.DMG} от {card.NameText.text}");
                    _battlecryDamage = 0;
                    
                    _cardSignalBus.CardDoBattlecryFire(new CardDoBattlecrySignal(EBattlecrySubStage.False));
                    break;
                }
            }
        }
    }
}