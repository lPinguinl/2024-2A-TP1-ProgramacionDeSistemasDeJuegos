using UnityEngine;

namespace Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyPool enemyPool;
        [SerializeField] private float spawnInterval = 1f;

        private void Start()
        {
            InvokeRepeating(nameof(SpawnEnemy), 0f, spawnInterval);
        }

        private void SpawnEnemy()
        {
            Vector3 spawnPosition = transform.position;
            enemyPool.GetEnemy(spawnPosition); // Spawn the enemy from the pool
        }
    }
}