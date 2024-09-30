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
        private EnemyPool enemyPool; 
        [SerializeField] private EnemyData enemyData;

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
            yield return null; 

            
            GameObject[] townCenters = GameObject.FindGameObjectsWithTag("TownCenter");
            if (townCenters.Length == 0)
            {
                Debug.LogError($"{name}: Found no TownCenters!! :(");
                yield break; 
            }
            
            GameObject randomTownCenter = townCenters[UnityEngine.Random.Range(0, townCenters.Length)];

            var destination = randomTownCenter.transform.position;
            destination.y = transform.position.y;

            agent.enabled = true; 
            agent.SetDestination(destination); 
            
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
                Die(); 
            }
        }

        private void OnCollisionStay(Collision other)
        {
            if (other.gameObject.CompareTag("TownCenter"))
            {
                other.gameObject.GetComponent<Structure>().TakeDamage(enemyData.damage); 
                StartCoroutine(AttackCooldown());
            }
        }

        private void Die()
        {
            OnDeath();
            enemyPool.ReturnEnemy(this); 
        }

        public void TakeDamage(int amount)
        {
            healthComponent.TakeDamage(amount);
        }

        private IEnumerator AttackCooldown()
        {
            yield return new WaitForSeconds(enemyData.attackCooldown); 
        }

        public override EnemyPrototype Clone()
        {
            Enemy clone = Instantiate(this); 
            clone.enemyData = this.enemyData; 
            return clone;
        }
    }
}
