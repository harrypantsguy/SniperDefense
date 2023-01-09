using UnityEngine;

namespace _Project.Codebase
{
    public class World : MonoSingleton<World>
    {
        public float width;
        public float height;

        public float WidthExtents => width / 2f;
        public float HeightExtents => height / 2f;

        [SerializeField] private SpriteRenderer _spriteRenderer;

        private void OnValidate()
        {
            UpdateSpriteRenderer();
        }

        private void Update()
        {
            UpdateSpriteRenderer();
        }

        private void UpdateSpriteRenderer()
        {
            Vector2 size = new Vector2(width, height);
            if (_spriteRenderer != null && _spriteRenderer.size != size)
                _spriteRenderer.size = size;
        }
    }
}