using Cards;

public class CardDoBattlecrySignal
{
    public readonly EBattlecrySubStage IsBattlecrySubStage;
    public readonly CardView Card;

    public CardDoBattlecrySignal(EBattlecrySubStage isBattlecrySubStage, CardView card)
    {
        IsBattlecrySubStage = isBattlecrySubStage;
        Card = card;
    }
}