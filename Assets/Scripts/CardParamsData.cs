namespace Cards
{
    public struct CardParamsData
    {
        public ushort Cost;
        public ushort Attack;
        public ushort Health;

        public CardParamsData(ushort cost, ushort attack, ushort health)
        {
            Cost = cost; Attack = attack; Health = health;
        }
    }
}