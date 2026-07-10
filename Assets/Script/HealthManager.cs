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

    [Header("Fade UI Components")]
    [SerializeField] private CanvasGroup cutsceneCanvasGroup;
    [SerializeField] private CanvasGroup retryBtnCanvasGroup;
    [SerializeField] private CanvasGroup mainMenuBtnCanvasGroup;
    [SerializeField] private float fadeSpeed = 1.5f; 

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource; // Komponen pemutar audio
    [SerializeField] private AudioClip failSound;     // Slot untuk memasukkan file audio gagal

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

        if (cutsceneCanvasGroup != null) cutsceneCanvasGroup.alpha = 0f;
        if (retryBtnCanvasGroup != null) retryBtnCanvasGroup.alpha = 0f;
        if (mainMenuBtnCanvasGroup != null) mainMenuBtnCanvasGroup.alpha = 0f;

        // Otomatis mengambil AudioSource jika lupa dipasang di Inspector
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

        if (healthBarRoot != null)
        {
            healthBarRoot.SetActive(false);
        }

        // Kritis: Mainkan backsound gagal seketika saat nyawa habis!
        if (audioSource != null && failSound != null)
        {
            audioSource.PlayOneShot(failSound);
        }

        StartCoroutine(PlayFadeInCutscene());
    }

    private IEnumerator PlayFadeInCutscene()
    {
        yield return new WaitForSeconds(0.3f); 

        while (cutsceneCanvasGroup.alpha < 1f)
        {
            cutsceneCanvasGroup.alpha = Mathf.MoveTowards(cutsceneCanvasGroup.alpha, 1f, fadeSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        while (retryBtnCanvasGroup.alpha < 1f || mainMenuBtnCanvasGroup.alpha < 1f)
        {
            retryBtnCanvasGroup.alpha = Mathf.MoveTowards(retryBtnCanvasGroup.alpha, 1f, fadeSpeed * Time.deltaTime);
            mainMenuBtnCanvasGroup.alpha = Mathf.MoveTowards(mainMenuBtnCanvasGroup.alpha, 1f, fadeSpeed * Time.deltaTime);
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