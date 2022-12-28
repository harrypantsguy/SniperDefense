using UnityEngine;

namespace _Project.Codebase
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;

        private World _world;
        private float _lastSpawnTime;
        private float _spawnDelay = 1f;

        private void Start()
        {
            _world = World.Singleton;
        }

        private void Update()
        {
            if (Time.time > _lastSpawnTime + _spawnDelay)
            {
                GameObject newEnemy = Instantiate(_prefab, new Vector3(_world.WidthExtents, 
                    Random.Range(-_world.HeightExtents, _world.HeightExtents)), Quaternion.identity);

                _lastSpawnTime = Time.time;
                _spawnDelay = 1f;
            }
        }
    }
}