using UnityEngine;

namespace _Project.Codebase.Enemies
{
    public abstract class AIBehavior
    {
        protected readonly AIController controller;
        private AIBehavior _nextBehavior;

        public Entity Entity { get; }
        public float ElapsedTime { get; private set; }
        public AIBehavior(AIController controller)
        {
            this.controller = controller;
            Entity = controller.Entity;
        }

        public virtual void OnEnter() {}

        public virtual void Tick(float deltaTime)
        {
            ElapsedTime += deltaTime;
        }

        public virtual void OnExit()
        {
            if (_nextBehavior != null)
                controller.SetState(_nextBehavior);
        }

        protected void MoveEnemy(Vector3 vector)
        {
            Entity.transform.position += vector;
        }
    }
}