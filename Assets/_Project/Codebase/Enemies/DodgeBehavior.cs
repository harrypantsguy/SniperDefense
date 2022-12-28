
using System.Runtime.Serialization;
using UnityEngine;

namespace _Project.Codebase.Enemies
{
    public class DodgeBehavior : AIBehavior
    {
        [SerializeField] private AnimationCurve _curve;
        
        private int _desiredDodgeCount;
        private int _direction;
        private int _dodgeCount;
        private float _dodgeTimer;
        private float _pauseTimer;
        private float _dodgeDistance;

        private DodgerEnemy _enemy;
        
        private const float DODGE_PERIOD = .4f;
        private const float PAUSE_PERIOD = .3f;

        public DodgeBehavior(AIController controller, int desiredDodgeCount) : base(controller)
        {
            _enemy = controller.Entity as DodgerEnemy;
            _desiredDodgeCount = desiredDodgeCount;
            ResetAndRandomizeDodge();
        }

        private void ResetAndRandomizeDodge()
        {
            _dodgeDistance = Random.Range(1.5f, 2.125f);

            if (Entity.transform.position.y + _dodgeDistance > World.Singleton.HeightExtents)
                _direction = -1;
            else if (Entity.transform.position.y - _dodgeDistance < -World.Singleton.HeightExtents)
                _direction = 1;
            else
                _direction = Random.Range(0, 2) == 0 ? -1 : 1;
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            bool paused = _pauseTimer < PAUSE_PERIOD;
            _enemy.DodgeLineEnabled = false;//paused;

            if (paused)
            {
                _enemy.DodgeLineTarget = new Vector2(0f, _direction * _dodgeDistance);
                Vector2 displacement = new Vector3(-.707f * deltaTime, _direction * .707f * deltaTime);
                _pauseTimer += deltaTime;
                MoveEnemy(displacement);
            }
            else
            {
                _dodgeTimer += deltaTime;

                float P = 2.75f;
                float T = DODGE_PERIOD;
                float D = _dodgeDistance;
                float M = D / (-T / P * (Mathf.Exp(-P) - 1f));

                float exponentComp = Mathf.Exp(-_dodgeTimer / (DODGE_PERIOD / P));

                MoveEnemy(new Vector3(-3f * deltaTime, M * exponentComp * deltaTime * _direction));
            }

            if (_dodgeTimer >= DODGE_PERIOD)
            {
                _dodgeTimer -= DODGE_PERIOD;
                _dodgeCount++;
                ResetAndRandomizeDodge();
            }

            if (_dodgeCount == _desiredDodgeCount)
                controller.SetState(new DodgerPursueTargetBehavior(controller));
            
        }
    }
}