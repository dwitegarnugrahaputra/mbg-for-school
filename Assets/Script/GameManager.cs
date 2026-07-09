using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private Slider healthSlider;

    [Header("Scene Settings")]
    [SerializeField] private string gameOverSceneName = "GameOverScene"; // Nama scene berita kriminal/satir lo

    private void Awake()
    {
        // Singleton Pattern: Memastikan hanya ada 1 GameManager di dalam game
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TriggerGameOver()
    {
        // Membuka scene baru berisi cutscene berita kriminal satir lo
        SceneManager.LoadScene(gameOverSceneName);
    }
}