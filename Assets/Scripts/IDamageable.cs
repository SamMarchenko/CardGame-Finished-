﻿namespace DefaultNamespace
{
    public interface IDamageable
    {
        public int GetDamage();
        public void ApplyDamage(int damage);

        public void SetCoolDownAttack(bool value);
    }
}