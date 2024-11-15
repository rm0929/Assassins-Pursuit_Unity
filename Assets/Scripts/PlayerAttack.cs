using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 15f; // Attack range in units
    public float attackDelay = 0f; // Delay between attacks to avoid spamming
    private float nextAttackTime = 0f;

    [SerializeField] private Animator animator; // Reference to the player's Animator
    [SerializeField] private GameObject projectilePrefab; // Reference to the projectile prefab
    [SerializeField] private Transform projectileSpawnPoint; // Point from where the projectile will be launched

    private void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            CheckForAttack();
        }
    }

    private void CheckForAttack()
    {
        // Find all enemies in the scene
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, attackRange);

        foreach (Collider enemy in enemiesInRange)
        {
            if (enemy.CompareTag("Enemy")) // Assuming the enemy has the tag "Enemy"
            {
                RaycastHit hit;
                Vector3 direction = enemy.transform.position - transform.position;

                // Raycast to check if there's a clear line of sight
                if (Physics.Raycast(transform.position, direction, out hit, attackRange))
                {
                    if (hit.collider.CompareTag("Enemy") && hit.collider.gameObject == enemy.gameObject)
                    {
                        Attack(enemy);
                    }
                }
            }
        }
    }

    private void Attack(Collider enemy)
    {
        // Make the player look at the enemy before attacking
        Vector3 direction = enemy.transform.position - transform.position;
        direction.y = 0; // Keep the player from tilting vertically (you can remove this if you want vertical rotation)
        transform.rotation = Quaternion.LookRotation(direction);

        // Play the attack animation
        animator.SetTrigger("Attack");

        // Set the time when the next attack can occur
        nextAttackTime = Time.time + attackDelay;
    }

    // Method called by the animation event to spawn the projectile at the right time in the animation
    public void SpawnProjectile()
    {
        // Find the first enemy within range
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, attackRange);

        foreach (Collider enemy in enemiesInRange)
        {
            if (enemy.CompareTag("Enemy"))
            {
                // Instantiate the projectile at the spawn point
                if (projectilePrefab != null && projectileSpawnPoint != null)
                {
                    GameObject homingProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

                    // Get the Projectile script and assign the target (enemy)
                    Projectile projectileScript = homingProjectile.GetComponent<Projectile>();
                    if (projectileScript != null)
                    {
                        projectileScript.target = enemy.transform; // Assign the target enemy
                    }
                }
                break; // Once a projectile is spawned, stop checking for others (optional)
            }
        }
    }
}
