using System;
using Cards;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private EGameStage _gameStage;
    private StageController _stageController;

    public Action StartHandChosen;
    public Action FinishMoving;

    public void Init(StageController stageController)
    {
        _stageController = stageController;
    }

    private void Start()
    {
        _gameStage = EGameStage.ChooseStartHand;
        _stageController.SetCurrentGameStage += OnUpdateCurrentGameStage;
    }
    
    void Update()
    {
        switch (_gameStage)
        {
            case EGameStage.ChooseStartHand:
                CheckStartHandStageInput();
                break;
            case EGameStage.Play:
                CheckPlayStageInput();
                break;
        }
    }

    private void CheckStartHandStageInput()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            StartHandChosen?.Invoke();
        }
    }

    private void CheckPlayStageInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            FinishMoving?.Invoke();
        }
    }
    private void OnUpdateCurrentGameStage(EGameStage stage)
    {
        _gameStage = stage;
    }
}
