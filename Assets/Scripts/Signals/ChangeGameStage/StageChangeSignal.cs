using Cards;

public class StageChangeSignal
{
    public readonly ECurrentStageType StageType;

    public StageChangeSignal(ECurrentStageType stageType)
    {
        StageType = stageType;
    }
}