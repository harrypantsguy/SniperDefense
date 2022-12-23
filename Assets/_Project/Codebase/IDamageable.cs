namespace _Project.Codebase
{
    public interface IDamageable
    {
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public void TakeDamage(DamageReport damageReport);
    }
}