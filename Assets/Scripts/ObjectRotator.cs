using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 100, 0); // Rotation speed for each axis (x, y, z)

    void Update()
    {
        // Rotate the object based on the rotation speed and deltaTime
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
