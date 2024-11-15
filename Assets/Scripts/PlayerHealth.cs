using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5; // Maximum health
    private int currentHealth;

    [SerializeField] private Slider healthBar; // Reference to the health bar slider in the UI
    [SerializeField] private GameObject gameOverCanvas; // Reference to the Game Over Canvas
    private Animator animator; // Reference to the Animator

    private bool isDying = false; // Tracks if the death sequence has started

    [SerializeField] private float additionalDelay = 1.0f; // Extra time before showing the Game Over Canvas

    private void Start()
    {
        currentHealth = maxHealth; // Initialize player's health to max
        UpdateHealthBar(); // Update health bar at the start
        animator = GetComponent<Animator>(); // Get the Animator component

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(false); // Hide Game Over canvas at the start
    }

    public void TakeDamage(int damage)
    {
        if (isDying) return; // Prevent further actions if the player is already dying

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
        isDying = true; // Mark the player as dying to avoid repeated triggers
        animator.SetTrigger("playerIsDead"); // Trigger the death animation
        StartCoroutine(ShowGameOverAfterAnimation());
    }

    private System.Collections.IEnumerator ShowGameOverAfterAnimation()
    {
        // Wait for the death animation to complete
        AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = animationState.length;

        yield return new WaitForSeconds(animationLength + additionalDelay); // Wait for animation and additional delay

        gameOverCanvas.SetActive(true); // Show the Game Over Canvas
        Time.timeScale = 0f; // Pause the game
    }

    public void RestartGame()
    {
        // Resume the game
        Time.timeScale = 1f;

        // Reload the first level (index 0 in the build settings)
        SceneManager.LoadScene(0);

        // Reset player health
        currentHealth = maxHealth;
        UpdateHealthBar();

        // Reset Animator to default state
        if (animator != null)
        {
            animator.Play("IdleAnimation"); // Replace "Idle" with your player's default animation state name
            animator.ResetTrigger("playerIsDead"); // Clear death animation trigger
        }

        // Hide the Game Over Canvas
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }

        // Reset death state
        isDying = false;
    }

}
