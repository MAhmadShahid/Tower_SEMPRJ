using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2Manager : MonoBehaviour
{
    public static Level2Manager Instance { get; private set; }

    public int collectedItems;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        collectedItems = 0;
    }

    public void CollectItem()
    {
        collectedItems++;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
