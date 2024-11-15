using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAIPatrol : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;

    [SerializeField] LayerMask groundLayer, playerLayer;
    Animator animator;

    // Box Collider for attack
    BoxCollider boxCollider;

    // Patrol
    Vector3 destPoint;
    bool walkpointSet;
    [SerializeField] float range;

    //state change
    [SerializeField] float sightRange, attackRange;
    bool playerInSight, playerInAttackRange;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();

        GameObject attackColliderObject = GameObject.FindGameObjectWithTag("attackCollider");

        if (attackColliderObject != null)
        {
            boxCollider = attackColliderObject.GetComponent<BoxCollider>();
            boxCollider.isTrigger = true; // Ensure it's set as a trigger
        }
        else
        {
            Debug.LogWarning("No BoxCollider with the tag 'attackCollider' found.");
        }
        
        // boxCollider = GetComponentInChildren<BoxCollider>();
        boxCollider.isTrigger = true; // Ensure it's set as a trigger

        if (boxCollider == null)
        {
            Debug.LogWarning("BoxCollider not found or not set as trigger.");
        }
    }

    void Update()
    {
        playerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);
        
        if (!playerInSight && !playerInAttackRange) Patrol();
        if (playerInSight && !playerInAttackRange) Chase();
        if (playerInSight && playerInAttackRange) Attack();
    }

    void Chase()
    {
        agent.SetDestination(player.transform.position);
    }

    void Attack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("EnemyAttackAnimation"))
        {
            animator.SetTrigger("Attack");
            agent.SetDestination(transform.position);
        }
    }

    void Patrol()
    {
        if (!walkpointSet) SearchForDest();
        if (walkpointSet) agent.SetDestination(destPoint);
        if (Vector3.Distance(transform.position, destPoint) < 10) walkpointSet = false;
    }

    void SearchForDest()
    {
        float z = Random.Range(-range, range);
        float x = Random.Range(-range, range);

        destPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        if (Physics.Raycast(destPoint, Vector3.down, groundLayer))
        {
            walkpointSet = true;
        }
    }

    void EnableAttack()
    {
        boxCollider.enabled = true;
        Debug.Log("Enabled Attack");
    }

    void DisableAttack()
    {
        boxCollider.enabled = false;
        Debug.Log("Disabled Attack");
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        Debug.Log("Triggered by: " + other.name); //w Check which object triggered the event
        if (other.CompareTag("Projectile"))
        {
            // Destroy the projectile that collided with the enemy
            Destroy(other.gameObject);

            // Debug message saying the enemy took damage
            Debug.Log("Enemy took 1 damage.");

            // You can also add logic to deal damage to the enemy here if required
            // For example, reducing the enemy's health or triggering a health system
        }

        var player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            playerHealth.TakeDamage(1); // Deal 1 damage to the player
            Debug.Log("Player hit by enemy, taking 1 damage.");
        }
    }
}
