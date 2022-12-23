using System.Collections.Generic;
using UnityEngine;

namespace _Project.Codebase
{
    public class Projectile : MonoBehaviour
    {
        [HideInInspector] public Vector2 velocity;
        public float damage;
        public float radius;
        public float speed;
        public float range;
        public int penetration;

        [SerializeField] private TrailRenderer _trail;
        [SerializeField] private SpriteRenderer _sprite;
        private Vector2 _fireLocation;
        private float VisualSpeed => velocity.magnitude * TimeController.TimeScale;
        protected bool _queuedToDestroy;
        private List<Collider2D> hitColliders = new List<Collider2D>();

        protected virtual void LateUpdate()
        {
            _sprite.enabled = VisualSpeed < 300;

            if (_queuedToDestroy)
            {
                Destroy();
                return;
            }
            
            transform.right = velocity.normalized;

            float remainingRange = range - Vector2.Distance
                                       (_fireLocation, transform.position);
            
            Vector2 displacement = Vector2.ClampMagnitude((Vector3)velocity * Time.deltaTime, remainingRange);

            if (remainingRange < .001f)
                _queuedToDestroy = true;
            
            if (TryCircleCast(transform.position, displacement, out RaycastHit2D hit))
            {
                if (hit.collider.TryGetComponent(out IDamageable damageable))
                {
                    DamageReport report = new DamageReport
                    {
                        damage = (int)damage,
                        direction = displacement.normalized,
                        impactLocation = hit.point
                    };
                    damageable.TakeDamage(report);
                }
                hitColliders.Add(hit.collider);
                
                Vector2 newPosition = hit.GetCollisionPos(radius);
                
                if (penetration == 0)
                {
                    transform.position = newPosition;
                    _queuedToDestroy = true;
                }
                else
                    penetration--;
                return;
            }
            
            transform.position += (Vector3)displacement;
        }

        protected bool TryCircleCast(Vector2 position, Vector2 displacement, out RaycastHit2D hit)
        {
            hit = default;
            RaycastHit2D[] hits = Physics2D.CircleCastAll(position, radius, displacement.normalized,
                displacement.magnitude, Layers.projectileHitMask);

            for (int i = 0; i < hits.Length; i++)
            {
                if (!hitColliders.Contains(hits[i].collider))
                {
                    hit = hits[i];
                }
            }
            
            if (!hit) return false;

            return true;
        }

        private void Destroy()
        {
            _trail.autodestruct = true;
            _trail.transform.SetParent(null);
            Destroy(gameObject);
        }

        public virtual void Fire(Vector2 position, Vector2 direction)
        {
            transform.position = position;
            _fireLocation = position;
            velocity = direction * speed;
        }

        public static Projectile FireProjectile(GameObject prefab, Vector2 pos, Vector2 direction)
        {
            GameObject newProjObj = Instantiate(prefab);
            if (!newProjObj.TryGetComponent(out Projectile newProj)) return null;

            newProj.Fire(pos, direction);
            return newProj;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}