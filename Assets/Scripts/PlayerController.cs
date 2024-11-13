using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private Transform spawnPoint;
    private Animator levelLoaderAnimator;
    public bool HasKey { get; set; } = false; // Tracks if player has picked up the key

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

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        spawnPoint = GameObject.FindWithTag("PlayerSpawnPoint")?.transform;

        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
        }
        else
        {
            Debug.LogWarning("Spawn point not found in scene: " + scene.name);
        }

        levelLoaderAnimator = GameObject.FindWithTag("LevelLoader")?.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
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
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
