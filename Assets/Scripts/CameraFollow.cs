using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset = new Vector3(0, 10, -10); // Default offset from the player
    public Vector3 cameraRotation = new Vector3(37.5f, 0, 0); // Default rotation for the camera

    void LateUpdate()
    {
        // Set the camera position to follow the player
        transform.position = playerTransform.position + offset;

        // Apply custom rotation to the camera
        transform.rotation = Quaternion.Euler(cameraRotation);
    }
}
