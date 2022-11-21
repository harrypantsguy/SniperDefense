using UnityEngine;

namespace _Project.Codebase
{
    [ExecuteAlways]
    public class World : MonoSingleton<World>
    {
        public float width;
        public float height;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        private void Update()
        {
            if (_spriteRenderer != null)
                _spriteRenderer.transform.localScale = new Vector3(width, height);
        }
    }
}