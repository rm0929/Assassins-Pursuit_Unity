using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Vector3 openPositionOffset = new Vector3(-2, 0, 0); // Offset to move door left
    public float openRadius = 3f; // Radius within which the door opens
    public float openSpeed = 2f; // Speed of door opening movement
    private bool isOpen = false;
    private bool hasPassedThrough = false; // Tracks if the player has used the key at this door
    private Vector3 closedPosition;
    private Vector3 openPosition;
    private Transform playerTransform;
    
    private void Start()
    {
        closedPosition = transform.position; // Store the door's closed position
        openPosition = closedPosition + openPositionOffset; // Calculate the open position
        playerTransform = GameObject.FindWithTag("Player").transform; // Find the player’s transform
    }

    private void Update()
    {
        if (PlayerController.Instance.HasKey && !isOpen && IsPlayerInRange())
        {
            isOpen = true; // Start the door opening process
        }

        if (isOpen && !hasPassedThrough)
        {
            SmoothOpenDoor(); // Smoothly open the door
        }
    }

    private bool IsPlayerInRange()
    {
        float distance = Vector3.Distance(playerTransform.position, transform.position);
        return distance <= openRadius; // Check if player is within the specified radius
    }

    private void SmoothOpenDoor()
    {
        transform.position = Vector3.Lerp(transform.position, openPosition, openSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, openPosition) < 0.01f)
        {
            transform.position = openPosition; // Snap to the open position
            isOpen = false; // Stop further opening movement
            hasPassedThrough = true; // Mark the door as used

            PlayerController.Instance.HasKey = false; // Remove the key from the player
        }
    }
}
