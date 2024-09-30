using System;
using System.Collections;
using Structures;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Health))]
    public class Enemy : EnemyPrototype
    {
        [SerializeField] private NavMeshAgent agent;

        private Health healthComponent;
        private EnemyPool enemyPool; // Reference to the enemy pool
        [SerializeField] private EnemyData enemyData; // Reference to the Scriptable Object

        public event Action OnSpawn = delegate { };
        public event Action OnDeath = delegate { };

        private void Reset() => FetchComponents();

        private void Awake()
        {
            FetchComponents();
            healthComponent = GetComponent<Health>();
            healthComponent.OnDeath += Die; // Subscribe to the death event
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
            yield return null; // Wait for a frame

            // Find all GameObjects with the tag "TownCenter"
            GameObject[] townCenters = GameObject.FindGameObjectsWithTag("TownCenter");
            if (townCenters.Length == 0)
            {
                Debug.LogError($"{name}: Found no TownCenters!! :(");
                yield break; // Exit if no TownCenters are found
            }

            // Select a random TownCenter from the list
            GameObject randomTownCenter = townCenters[UnityEngine.Random.Range(0, townCenters.Length)];

            var destination = randomTownCenter.transform.position;
            destination.y = transform.position.y;

            agent.enabled = true; // Ensure NavMeshAgent is enabled
            agent.SetDestination(destination); // Set the destination
            
            Debug.Log($"{name} is moving towards {randomTownCenter.name} at {destination}");
            
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
                Debug.Log($"{name}: I'll attack the TownCenter!");
                Die(); // Call Die when reaching the destination
            }
        }

        private void OnCollisionStay(Collision other)
        {
            if (other.gameObject.CompareTag("TownCenter"))
            {
                other.gameObject.GetComponent<Structure>().TakeDamage(enemyData.damage); // Use Flyweight data from Scriptable Object
                StartCoroutine(AttackCooldown());
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
        }

        private IEnumerator AttackCooldown()
        {
            yield return new WaitForSeconds(enemyData.attackCooldown); // Use Flyweight data from Scriptable Object
        }

        public override EnemyPrototype Clone()
        {
            Enemy clone = Instantiate(this); // Create a new instance of the enemy
            clone.enemyData = this.enemyData; // Share the same data reference
            return clone;
        }
    }
}
