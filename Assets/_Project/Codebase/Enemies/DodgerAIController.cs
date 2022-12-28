namespace _Project.Codebase.Enemies
{
    public class DodgerAIController : AIController
    {
        public DodgerAIController(Entity entity) : base(entity)
        {
            SetState(new DodgerPursueTargetBehavior(this));
        }
    }
}