using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    public float currentHealth { get; private set; } // Make currentHealth a property

    public event System.Action OnDeath; // Event to notify when health reaches zero

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0; // Prevent negative health

        if (currentHealth <= 0)
        {
            OnDeath?.Invoke(); // Invoke death event
        }
    }
}