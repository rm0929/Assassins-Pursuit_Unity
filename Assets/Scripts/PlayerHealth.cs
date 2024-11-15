using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5; // Maximum health
    private int currentHealth;

    [SerializeField] private Slider healthBar; // Reference to the health bar slider in the UI
    private Animator animator; // Reference to the Animator

    private void Start()
    {
        currentHealth = maxHealth; // Initialize player's health to max
        UpdateHealthBar(); // Update health bar at the start
        animator = GetComponent<Animator>(); // Get the Animator component
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
        animator.SetTrigger("playerIsDead"); // Trigger the death animation
        // Add any additional code here, like disabling player movement or showing a game over screen
    }
}
