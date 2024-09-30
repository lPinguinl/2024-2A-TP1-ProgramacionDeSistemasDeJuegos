using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class EnemyPool : MonoBehaviour
    {
        [SerializeField] private Enemy enemyPrefab;
        [SerializeField] private int poolSize = 160;

        private List<Enemy> enemies;

        private void Awake()
        {
            enemies = new List<Enemy>();
            for (int i = 0; i < poolSize; i++)
            {
                Enemy enemy = Instantiate(enemyPrefab);
                enemy.gameObject.SetActive(false); // Set inactive
                enemies.Add(enemy);
            }
        }

        public Enemy GetEnemy(Vector3 position)
        {
            foreach (Enemy enemy in enemies)
            {
                if (!enemy.gameObject.activeInHierarchy)
                {
                    enemy.transform.position = position;
                    enemy.gameObject.SetActive(true);
                    return enemy; // Return the reused enemy
                }
            }

            // If no inactive enemies are available, use the prototype method to create a new one
            EnemyPrototype newEnemy = enemyPrefab.Clone();
            newEnemy.transform.position = position;
            enemies.Add((Enemy)newEnemy);
            return (Enemy)newEnemy;
        }

        public void ReturnEnemy(Enemy enemy)
        {
            enemy.gameObject.SetActive(false); // Deactivate the enemy
        }
    }
}