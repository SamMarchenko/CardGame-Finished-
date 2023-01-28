namespace DefaultNamespace
{
    public class DamageController
    {
        public IDamageable Attacking;
        public IDamageable Attacked;
        
        
        

        private void Attack()
        {
           var damage =  Attacking.ApplyDamage();
           Attacked.TakeDamage(damage);

           damage = Attacked.ApplyDamage();
           Attacking.TakeDamage(damage);
        }
    }
}