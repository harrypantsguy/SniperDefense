using UnityEngine;

namespace _Project.Codebase
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;

        private World _world;
        private float _lastSpawnTime;

        private void Start()
        {
            _world = World.Singleton;
        }

        private void Update()
        {
            float spawnDelay = 1f;
            if (Time.time > _lastSpawnTime + spawnDelay)
            {
                GameObject newEnemy = Instantiate(_prefab, new Vector3(_world.WidthExtents, 
                    Random.Range(-_world.HeightExtents, _world.HeightExtents)), Quaternion.identity);

                _lastSpawnTime = Time.time;
            }
        }
    }
}