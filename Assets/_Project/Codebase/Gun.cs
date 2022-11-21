using UnityEngine;

namespace _Project.Codebase
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private float _range;
        public Vector2 targetPos;
        
        private void Update()
        {
            Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPositions(new Vector3[]{_lineRenderer.transform.position, (Vector2)_lineRenderer.transform.position
                                                                                       + direction * _range});
        }
    }
}