using System.Collections;
using Enemies;
using UnityEngine;

namespace Structures
{
    [RequireComponent(typeof(Health))]
    public class Structure : MonoBehaviour
    {
        private Health healthComponent;
        
        [SerializeField]private int damage;

        [SerializeField]private float attackCooldown;

        private void OnEnable()
        {
            healthComponent = GetComponent<Health>();
            healthComponent.OnDeath += Die; // Subscribe to death event
        }

        private void OnDisable()
        {
            healthComponent.OnDeath -= Die; // Unsubscribe to avoid memory leaks
        }

        public void TakeDamage(int amount)
        {
            healthComponent.TakeDamage(amount);
        }
        
        private void OnCollisionStay(Collision other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                
                StartCoroutine(AttackCooldown());
            }
        }
        
        IEnumerator AttackCooldown()
        {
            yield return new WaitForSeconds(attackCooldown);
        }

        private void Die()
        {
            Debug.Log($"{name} has been destroyed!");
            Destroy(gameObject); // Destroy the structure
        }
    }
}