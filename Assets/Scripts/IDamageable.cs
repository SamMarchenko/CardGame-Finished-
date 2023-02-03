namespace DefaultNamespace
{
    public interface IDamageable
    {
        public int GetActualDamage();
        public void ApplyDamage(int damage);

        public void SetCoolDownAttack(bool value);
    }
}