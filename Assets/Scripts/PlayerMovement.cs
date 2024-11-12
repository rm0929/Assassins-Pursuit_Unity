using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    public float moveSpeed = 5f;
    public float turnSpeed = 300f;
    public float jumpForce = 5f;

    private Rigidbody rb;
    private bool isGrounded;

    private Transform spawnPoint; // Will store the spawn point
    private Animator levelLoaderAnimator; // Reference to Animator

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Subscribe to scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Called after a scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find spawn point after scene is fully loaded
        spawnPoint = GameObject.FindWithTag("PlayerSpawnPoint")?.transform;

        if (spawnPoint != null)
        {
            // Set player position to spawn point at the start of each level
            transform.position = spawnPoint.position;
        }
        else
        {
            Debug.LogWarning("Spawn point not found in scene: " + scene.name);
        }

        // Find the LevelLoader Animator in the scene
        levelLoaderAnimator = GameObject.FindWithTag("LevelLoader")?.GetComponent<Animator>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Ensure that spawn point is found when the scene first starts.
        if (spawnPoint == null)
        {
            spawnPoint = GameObject.FindWithTag("PlayerSpawnPoint")?.transform;
        }
        else
        {
            transform.position = spawnPoint.position;
        }
    }

    void Update()
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
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            Vector3 move = transform.forward * moveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + move);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            // Trigger the animation when the finish line is reached
            if (levelLoaderAnimator != null)
            {
                levelLoaderAnimator.SetTrigger("Start");
            }

            LoadNextLevel();
        }
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    private void OnDestroy()
    {
        // Unsubscribe from the scene loaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
