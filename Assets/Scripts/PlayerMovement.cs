using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 300f;
    public float jumpForce = 5f;

    private Rigidbody rb;
    private bool isGrounded;
    private Animator animator; // Reference to the Animator
    private bool playerIsRunning; // Boolean to control the running animation

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); // Get the Animator component attached to the player
    }

    private void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            // Set the playerIsRunning boolean based on movement
            playerIsRunning = true;

            // Rotate the player to face the move direction
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            // Move the player
            Vector3 move = transform.forward * moveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + move);
        }
        else
        {
            // Stop the running animation when no movement
            playerIsRunning = false;
        }

        // Pass the playerIsRunning value to the Animator immediately
        animator.SetBool("playerIsRunning", playerIsRunning);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
