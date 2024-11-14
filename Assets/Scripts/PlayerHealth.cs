using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5; // Maximum health
    private int currentHealth;

    [SerializeField] private Slider healthBar; // Reference to the health bar slider in the UI

    private void Start()
    {
        currentHealth = maxHealth; // Initialize player's health to max
        UpdateHealthBar(); // Update health bar at the start
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health doesn't drop below 0
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth / maxHealth;
        }
        else
        {
            Debug.LogWarning("Health bar reference is missing.");
        }
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        // Add code here to handle player death (e.g., restart level, show game over screen)
    }
}
