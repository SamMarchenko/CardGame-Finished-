using System;
using Cards;
using OneLine;

namespace DefaultNamespace
{
    [Serializable]
    public class BattlecriesParameters
    {
        [Width(50)] public int ID;
        [Width(70)] public EBattlecryAction Action;
        [Width(50)] public int DMG;
        [Width(50)] public int HP;
        [Width(70)] public int SummonId;
        [Width(50)] public EBattlecryTarget Target;
    }
}