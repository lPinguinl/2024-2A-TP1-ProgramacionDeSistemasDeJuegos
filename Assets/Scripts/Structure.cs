using UnityEngine;

namespace Structures
{
    [RequireComponent(typeof(Health))] // Ensure Health component is also required
    public class Structure : MonoBehaviour
    {
        private Health healthComponent; // Reference to the Health component

        private void Awake()
        {
            healthComponent = GetComponent<Health>(); // Get the Health component
        }

        // Example method for taking damage from an enemy
        public void TakeDamage(int amount)
        {
            healthComponent.TakeDamage(amount);
            if (healthComponent.CurrentHealth <= 0)
            {
                Die(); // Call Die if health is zero or below
            }
        }

        private void Die()
        {
            // Logic for what happens when the structure dies
            Debug.Log($"{name} has been destroyed!");
            Destroy(gameObject); // Destroy the structure object
        }
    }
}