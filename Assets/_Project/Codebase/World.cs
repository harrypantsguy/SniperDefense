using UnityEngine;

namespace _Project.Codebase
{
    [ExecuteAlways]
    public class World : MonoSingleton<World>
    {
        public float width;
        public float height;

        public float WidthExtents => width / 2f;
        public float HeightExtents => height / 2f;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        private void Update()
        {
            if (_spriteRenderer != null)
                _spriteRenderer.size = new Vector2(width, height);
        }
    }
}