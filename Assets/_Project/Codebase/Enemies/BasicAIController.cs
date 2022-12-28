namespace _Project.Codebase.Enemies
{
    public class BasicAIController : AIController
    {
        public BasicAIController(Entity entity) : base(entity)
        {
            SetState(new PursueTargetBehavior(this));
        }
    }
}