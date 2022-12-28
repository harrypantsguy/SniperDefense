using UnityEngine;

namespace _Project.Codebase.Enemies
{
    public abstract class AIController : EntityComponent
    {
        public AIBehavior AIBehavior { get; set; }
        public AIController SubStateController { get; set; }

        protected AIController(Entity entity) : base(entity)
        {
        }
        
        public void SetState(AIBehavior behavior)
        {
            AIBehavior?.OnExit();
            
            AIBehavior = behavior;
            if (behavior == null) return;

            AIBehavior.OnEnter();
        }

        public override void Update(float deltaTime)
        {
            SubStateController?.Update(deltaTime);
            AIBehavior?.Tick(Time.deltaTime);
        }
    }
}