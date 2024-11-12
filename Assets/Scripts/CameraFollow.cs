using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance;

    public Vector3 offset = new Vector3(0, 10, -10);
    public Vector3 cameraRotation = new Vector3(37.5f, 0, 0);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Persist camera across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LateUpdate()
    {
        if (PlayerMovement.Instance != null)
        {
            transform.position = PlayerMovement.Instance.transform.position + offset;
            transform.rotation = Quaternion.Euler(cameraRotation);
        }
    }
}
