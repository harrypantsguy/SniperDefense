
using UnityEngine;

namespace _Project.Codebase.Enemies
{
    public class DodgerPursueTargetBehavior : AIBehavior
    {
        public DodgerPursueTargetBehavior(AIController controller) : base(controller)
        {
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            MoveEnemy(new Vector3(-5f * deltaTime, 0f, 0));

            if (ElapsedTime > 2f)
                controller.SetState(new DodgeBehavior(controller, 1));
        }
    }
}