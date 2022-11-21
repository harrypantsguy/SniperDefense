using UnityEngine;

namespace _Project.Codebase
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;

        private float _lastSpawnTime;

        private void Update()
        {
            float spawnDelay = .75f;
            if (Time.time > _lastSpawnTime + spawnDelay)
            {
                GameObject newEnemy = Instantiate(_prefab, transform.position, Quaternion.identity);
            }
        }
    }
}