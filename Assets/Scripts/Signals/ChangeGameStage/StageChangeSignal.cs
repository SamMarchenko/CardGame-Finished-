using Cards;

public class StageChangeSignal
{
    private readonly ECurrentStageType _stageType;

    public StageChangeSignal(ECurrentStageType stageType)
    {
        _stageType = stageType;
    }
}