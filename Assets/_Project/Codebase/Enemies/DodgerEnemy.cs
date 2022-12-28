using UnityEngine;

namespace _Project.Codebase.Enemies
{
    public class DodgerEnemy : Enemy
    {
        [SerializeField] private LineRenderer _lineRenderer;

        public bool DodgeLineEnabled
        {
            set => _lineRenderer.enabled = value;
        }
        public Vector2 DodgeLineTarget
        {
            set => _lineRenderer.SetPosition(1, value);
        }
        protected override void Start()
        {
            base.Start();
            
            entityComponents.Add(new DodgerAIController(this));
        }
    }
}