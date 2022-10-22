using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [SerializeField] private Player _whoseMove;
    [SerializeField] private InputManager _inputManager;
    
    private Player _player1;
    private Player _player2;
    private EGameStage _gameStage = EGameStage.ChooseStartHand;
    
    public Player WhoseMove => _whoseMove;
    public Action<EGameStage> SetCurrentGameStage;
    public Action<Player> SetTurnMoving;

    private void Awake()
    {
        _inputManager.Init(this);
    }

    public void Init(Player player1, Player player2)
    {
        _player1 = player1;
        _player2 = player2;
        _whoseMove = _player1;
    }

    private void Start()
    {
        DebugLogCurrentStage();
        Subscribe();
        SetCurrentGameStage?.Invoke(_gameStage);
        SetTurnMoving?.Invoke(_whoseMove);
    }

    private void DebugLogCurrentStage()
    {
        Debug.Log($"Стадия = {_gameStage}, игрок = {_whoseMove}");
    }
    
    private void Subscribe()
    {
        _inputManager.FinishMoving += OnFinishMoving;
        _inputManager.StartHandChosen += OnStartHandChosen;
    }

    private void OnStartHandChosen()
    {
        Debug.Log($"{_whoseMove.name} выбрал стартовую руку");
        ChangeMover();
        
    }

    private void OnFinishMoving()
    {
        throw new NotImplementedException();
    }

    private void ChangeMover()
    {
        if (_whoseMove == _player1)
        {
            _whoseMove = _player2;
        }
        else if (_whoseMove == _player2)
        {
            _whoseMove = _player1;
            _gameStage = ChangeGameStage(EGameStage.Play);
            SetCurrentGameStage?.Invoke(_gameStage);
        }
        else
        {
            Debug.Log("Error!!!! Не определен игрок, чей ход.");
        }
        SetTurnMoving?.Invoke(_whoseMove);
    }

    private EGameStage ChangeGameStage(EGameStage stage)
    {
       return _gameStage = stage;
    }
}
