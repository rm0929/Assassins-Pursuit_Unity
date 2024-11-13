using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.Instance.HasKey = true; // Set HasKey to true on the PlayerController
            Destroy(gameObject); // Destroy the key object after pickup
        }
    }
}
