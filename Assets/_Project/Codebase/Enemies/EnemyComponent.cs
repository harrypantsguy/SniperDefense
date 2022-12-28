namespace _Project.Codebase.Enemies
{
    public class EnemyComponent : EntityComponent
    {
        protected Enemy Enemy { get; }
        
        public EnemyComponent(Entity entity) : base(entity)
        {
            Enemy = (Enemy)entity;
        }
    }
}