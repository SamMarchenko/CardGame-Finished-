using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [SerializeField] private ETurn _whoseMove = ETurn.First;
    public ETurn WhoseMove 
    {
        get => _whoseMove;
        set => _whoseMove = value;
    }
    private Player _player1;
    private Player _player2;

    public Action<Player> ChooseStartHand;

    public void Init(Player player1, Player player2)
    {
        _player1 = player1;
        _player2 = player2;
    }

    public void PlayerWantTakeCardFromDeck(Player player)
    {
        if (_player1.Stage == EGameStage.ChooseStartHand)
        {
            //передать команду на показ ему трех карт с возможностью выбора
            ChooseStartHand?.Invoke(player);
        }
        if (_player1.Stage == EGameStage.Move)
        {
            //передать разрешение на выдачу одной карты
        }
        else
        {
          //не его ход   
        }
    }

    public void UpdateStageStatus()
    {
        switch (_whoseMove)
        {
            case ETurn.First:
                switch (_player1.Stage)
                {
                    case EGameStage.ChooseStartHand:
                        _player1.Stage = EGameStage.Wait;
                        _player2.Stage = EGameStage.ChooseStartHand;
                        break;
                    case EGameStage.Move:
                        _player1.Stage = EGameStage.Wait;
                        _player2.Stage = EGameStage.Move;
                        break;
                    case EGameStage.Wait:
                        _player1.Stage = EGameStage.Move;
                        _player2.Stage = EGameStage.Wait;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                break;
            case ETurn.Second:
                switch (_player2.Stage)
                {
                    case EGameStage.ChooseStartHand:
                        _player1.Stage = EGameStage.Move;
                        _player2.Stage = EGameStage.Wait;
                        break;
                    case EGameStage.Move:
                        _player1.Stage = EGameStage.Move;
                        _player2.Stage = EGameStage.Wait;
                        break;
                    case EGameStage.Wait:
                        _player1.Stage = EGameStage.Wait;
                        _player2.Stage = EGameStage.Move;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
