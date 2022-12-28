namespace _Project.Codebase.Enemies
{
    public class EntityComponent
    {
        public Entity Entity { get; }
        public EntityComponent(Entity entity)
        {
            Entity = entity;
            Start();
        }

        protected virtual void Start() {}

        public virtual void Update(float deltaTime) {}
    }
}