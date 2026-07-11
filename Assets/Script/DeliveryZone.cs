using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Wajib ditambahin buat sistem timer/fade

public class DeliveryZone : MonoBehaviour
{
    [Header("Zone Settings")]
    [SerializeField] private float requiredStopTime = 3f;
    private float currentStopTime = 0f;
    private bool isDelivered = false;
    
    [SerializeField] private Renderer zoneRenderer;
    [SerializeField] private Color successColor = Color.green;

    [Header("UI Settings")]
    [SerializeField] private GameObject successPanel;
    [SerializeField] private CanvasGroup successCanvasGroup; // Slot baru untuk Fade
    [SerializeField] private float fadeSpeed = 1.5f; // Kecepatan Fade
    [SerializeField] private GameObject[] uiElementsToHide; 
    
    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource; // Slot buat corong suara
    [SerializeField] private AudioClip successSound;  // Slot buat lagu menangnya

    [Header("Scene Settings")]
    [SerializeField] private string levelSelectionSceneName = "LevelSelection"; 
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private void Start()
    {
        if (successPanel != null) successPanel.SetActive(false);
        if (successCanvasGroup != null) successCanvasGroup.alpha = 0f;
        
        if (zoneRenderer == null) zoneRenderer = GetComponent<Renderer>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (isDelivered) return;

        if (other.CompareTag("Player"))
        {
            Rigidbody playerRb = other.attachedRigidbody;
            
            if (playerRb != null && playerRb.linearVelocity.magnitude < 0.1f)
            {
                currentStopTime += Time.deltaTime;
                
                if (currentStopTime >= requiredStopTime)
                {
                    TriggerSuccess(other.gameObject);
                }
            }
            else
            {
                currentStopTime = 0f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentStopTime = 0f;
        }
    }

    private void TriggerSuccess(GameObject player)
    {
        isDelivered = true;

        if (zoneRenderer != null)
        {
            zoneRenderer.material.color = successColor;
        }

        ArcadeCarController carScript = player.GetComponent<ArcadeCarController>();
        if (carScript != null) carScript.enabled = false;

        // MATIKAN SEMUA TOMBOL & UI
        if (uiElementsToHide != null)
        {
            foreach (GameObject ui in uiElementsToHide)
            {
                if (ui != null) ui.SetActive(false);
            }
        }

        // MAINKAN SUARA MENANG
        if (audioSource != null && successSound != null)
        {
            audioSource.PlayOneShot(successSound);
        }

        PlayerPrefs.SetInt("Level2_Unlocked", 1);
        PlayerPrefs.Save();

        // JALANKAN ANIMASI FADE
        StartCoroutine(FadeInSuccess());
    }

    private IEnumerator FadeInSuccess()
    {
        if (successPanel != null) successPanel.SetActive(true);
        if (successCanvasGroup != null) successCanvasGroup.alpha = 0f;

        while (successCanvasGroup != null && successCanvasGroup.alpha < 1f)
        {
            successCanvasGroup.alpha = Mathf.MoveTowards(successCanvasGroup.alpha, 1f, fadeSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void Btn_Continue()
    {
        SceneManager.LoadScene(levelSelectionSceneName);
    }

    public void Btn_Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Btn_MainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}