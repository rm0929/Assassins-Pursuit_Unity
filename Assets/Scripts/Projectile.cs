using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f; // Speed of the projectile
    public Transform target; // The target enemy the projectile will home in on

    private void Start()
    {
        // If the target is assigned, start moving towards the target
        if (target != null)
        {
            // Set the target's Y position to the projectile's Y to maintain height
            Vector3 targetPosition = target.position;
            targetPosition.y = transform.position.y; // Maintain the projectile's height (no vertical movement)

            // Get the direction towards the target, ignoring vertical displacement
            Vector3 direction = (targetPosition - transform.position).normalized;

            // Apply the velocity towards the target
            GetComponent<Rigidbody>().velocity = direction * speed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log($"Enemy hit by projectile: {collision.gameObject.name}");
            Destroy(gameObject); // Destroy the projectile on impact
        }
        else
        {
            Debug.Log($"Projectile collided with: {collision.gameObject.name}");
        }
    }
}
