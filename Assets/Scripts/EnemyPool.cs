using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class EnemyPool : MonoBehaviour
    {
        [SerializeField] private Enemy enemyPrefab;
        [SerializeField] private int poolSize = 10;
        private List<Enemy> enemies;

        private void Awake()
        {
            enemies = new List<Enemy>();
            for (int i = 0; i < poolSize; i++)
            {
                Enemy enemy = Instantiate(enemyPrefab);
                enemy.gameObject.SetActive(false);
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
                    return enemy;
                }
            }

            // If no inactive enemies are available, create a new one
            Enemy newEnemy = Instantiate(enemyPrefab, position, Quaternion.identity);
            enemies.Add(newEnemy);
            return newEnemy;
        }

        public void ReturnEnemy(Enemy enemy)
        {
            enemy.gameObject.SetActive(false);
        }
    }
}