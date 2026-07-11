using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject healthBarRoot; 
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [Header("Damage Settings")]
    [SerializeField] private float damageAmount = 20f;
    [SerializeField] private float damageCooldown = 1f;
    private float cooldownTimer;

    [Header("GameOver UI")]
    [SerializeField] private GameObject gameOverPanel; 
    [SerializeField] private CanvasGroup gameOverCanvasGroup; 
    
    // KITA GANTI JADI ARRAY BIAR BISA MASUKIN BANYAK TOMBOL SEKALIGUS
    [SerializeField] private GameObject[] uiElementsToHide; 
    
    [SerializeField] private float fadeSpeed = 1.5f;

    [Header("Audio Settings (SFX Tabrakan & Game Over)")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip failSound;     
    [SerializeField] private AudioClip hitSound;      

    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        
        if (healthSlider != null)
        {
            healthSlider.minValue = 0f;
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (gameOverCanvasGroup != null) gameOverCanvasGroup.alpha = 0f;

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Obstacle") && cooldownTimer <= 0)
        {
            if (audioSource != null && hitSound != null)
            {
                audioSource.PlayOneShot(hitSound);
            }

            TakeDamage(damageAmount);
            cooldownTimer = damageCooldown;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0 && !isDead)
        {
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        isDead = true;
        
        ArcadeCarController carScript = GetComponent<ArcadeCarController>();
        if (carScript != null) carScript.enabled = false;

        if (healthBarRoot != null) healthBarRoot.SetActive(false);
        
        // LOOPING UNTUK MEMATIKAN SEMUA TOMBOL YANG ADA DI DALAM DAFTAR
        if (uiElementsToHide != null)
        {
            foreach (GameObject ui in uiElementsToHide)
            {
                if (ui != null) ui.SetActive(false);
            }
        }

        if (audioSource != null && failSound != null)
        {
            audioSource.PlayOneShot(failSound);
        }

        StartCoroutine(FadeInGameOver());
    }

    private IEnumerator FadeInGameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (gameOverCanvasGroup != null) gameOverCanvasGroup.alpha = 0f;

        while (gameOverCanvasGroup != null && gameOverCanvasGroup.alpha < 1f)
        {
            gameOverCanvasGroup.alpha = Mathf.MoveTowards(gameOverCanvasGroup.alpha, 1f, fadeSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); 
    }
}