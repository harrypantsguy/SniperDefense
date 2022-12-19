using UnityEngine;
using UnityEngine.UI;

namespace _Project.Codebase.UI
{
    public class MiniMap : MonoBehaviour
    {
        [SerializeField] private Image _groundImage;

        public float scaleFactor;
        
        private World _world;
        private void Start()
        {
            _world = World.Singleton;
        }

        private void Update()
        {
            _groundImage.rectTransform.sizeDelta = new Vector2(_world
                .width * scaleFactor, _world.height * scaleFactor);
        }
    }
}