using UnityEngine;

namespace _Project.Codebase.Enemies
{
    public class PursueTargetBehavior : AIBehavior
    {
        public PursueTargetBehavior(AIController controller) : base
        (controller)
        {
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            MoveEnemy(new Vector3(-5f * deltaTime, 0f));
        }
    }
}