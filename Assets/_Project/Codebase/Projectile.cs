using UnityEngine;

namespace _Project.Codebase
{
    public class Projectile : MonoBehaviour
    {
        [HideInInspector] public Vector2 velocity;
        public Vector2 direction;
        public float speed;

        private bool _queuedToDestroy;

        private void Start()
        {
            velocity = speed * direction;
        }

        private void Update()
        {
            if (_queuedToDestroy)
                Destroy(gameObject);
            transform.right = direction;
        }

        private void FixedUpdate()
        {
            transform.position += (Vector3)velocity * Time.fixedDeltaTime;
        }

        public static Projectile FireProjectile(GameObject prefab, Vector2 pos, Vector2 direction)
        {
            GameObject newProjObj = Instantiate(prefab);
            if (!newProjObj.TryGetComponent(out Projectile newProj)) return null;

            newProj.transform.position = pos;
            newProj.direction = direction;
            return newProj;
        }
    }
}