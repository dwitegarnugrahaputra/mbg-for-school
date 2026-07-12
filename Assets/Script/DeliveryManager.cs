using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class DeliveryManager : MonoBehaviour
{
    [Header("Progress Settings")]
    public int totalZones = 2; // Total target di level ini
    private int completedZones = 0;
    public Text progressText; // Slot buat teks "Delivery Zone 0/2"

    [Header("Success Panel Settings")]
    [SerializeField] private GameObject successPanel;
    [SerializeField] private CanvasGroup successCanvasGroup;
    [SerializeField] private float fadeSpeed = 1.5f;
    [SerializeField] private GameObject[] uiElementsToHide;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip successSound;

    [Header("Scene Settings")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string nextLevelSceneName = "Level 2"; // Ganti sesuai nama scene level berikutnya
    private void Start()
    {
        UpdateUI(); 
        if (successPanel != null) successPanel.SetActive(false);
        if (successCanvasGroup != null) successCanvasGroup.alpha = 0f;
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    // Fungsi ini dipanggil sama kotak merah kalau udah hijau
    public void RecordDelivery(GameObject player)
    {
        completedZones++;
        UpdateUI();

        // Cek kalau semua pengiriman udah beres
        if (completedZones >= totalZones)
        {
            LevelComplete(player);
        }
    }

    private void UpdateUI()
    {
        if (progressText != null)
        {
            progressText.text = "Delivery Zone " + completedZones + "/" + totalZones;
        }
    }

    private void LevelComplete(GameObject player)
    {
        // 1. Matiin kontrol mobil
        ArcadeCarController carScript = player.GetComponent<ArcadeCarController>();
        if (carScript != null) carScript.enabled = false;

        // 2. Sembunyiin UI tombol
        if (uiElementsToHide != null)
        {
            foreach (GameObject ui in uiElementsToHide)
            {
                if (ui != null) ui.SetActive(false);
            }
        }

        // 3. Mainkan audio menang
        if (audioSource != null && successSound != null)
        {
            audioSource.PlayOneShot(successSound);
        }

        // 4. Munculin Panel
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

    // Fungsi buat tombol Restart & Main Menu
    public void Btn_Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Btn_MainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void Btn_Continue()
    {
        // Normalin waktu lagi biar game nggak nge-freeze di level selanjutnya
        Time.timeScale = 1f; 

        // Pindah ke level selanjutnya
        SceneManager.LoadScene(nextLevelSceneName);
    }
}