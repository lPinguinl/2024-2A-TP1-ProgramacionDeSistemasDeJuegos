using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Health))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;

        private Health healthComponent; 
        private EnemyPool enemyPool; // Reference to the enemy pool

        public event Action OnSpawn = delegate { };
        public event Action OnDeath = delegate { };

        private void Reset() => FetchComponents();

        private void Awake()
        {
            FetchComponents();
            healthComponent = GetComponent<Health>();
            enemyPool = FindObjectOfType<EnemyPool>(); // Find the enemy pool
        }

        private void FetchComponents()
        {
            agent ??= GetComponent<NavMeshAgent>();
        }

        private void OnEnable()
        {
            StartCoroutine(InitializePath());
        }

        private IEnumerator InitializePath()
        {
            yield return null; // Wait for a frame to ensure the enemy is active on the NavMesh

            var townCenter = GameObject.FindGameObjectWithTag("TownCenter");
            if (townCenter == null)
            {
                Debug.LogError($"{name}: Found no TownCenter!! :(");
                yield break; // Exit if TownCenter is not found
            }

            var destination = townCenter.transform.position;
            destination.y = transform.position.y; // Adjust Y position if necessary

            agent.enabled = true; // Ensure the NavMeshAgent is enabled
            agent.SetDestination(destination); // Set the destination
            
            Debug.Log($"{name} is moving towards {destination}");
            
            StartCoroutine(AlertSpawn());
        }

        private IEnumerator AlertSpawn()
        {
            yield return null;
            OnSpawn();
        }

        private void Update()
        {
            if (agent.hasPath && Vector3.Distance(transform.position, agent.destination) <= agent.stoppingDistance)
            {
                Debug.Log($"{name}: I'll die for my people!");
                Die();
            }
        }

        private void Die()
        {
            OnDeath();
            enemyPool.ReturnEnemy(this); // Return the enemy to the pool
        }

        public void TakeDamage(int amount)
        {
            healthComponent.TakeDamage(amount);
            if (healthComponent.CurrentHealth <= 0)
            {
                Die(); 
            }
        }
    }
}
