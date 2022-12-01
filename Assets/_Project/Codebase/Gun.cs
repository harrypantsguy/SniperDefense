using UnityEngine;

namespace _Project.Codebase
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] public Transform _projectileSource;
        [SerializeField] private float _range;
        public Vector2 targetPos;
        public GameObject _projectileFab;
        
        private void Update()
        {
            Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPositions(new Vector3[]{_lineRenderer.transform.position, (Vector2)_lineRenderer.transform.position
                                                                                       + direction * _range});
        }

        public void Fire()
        {
            transform.right = (targetPos - (Vector2)transform.position).normalized;
            if (_projectileFab == null) return;
            
            Projectile.FireProjectile(_projectileFab, _projectileSource.position, _projectileSource.right);
        }
    }
}