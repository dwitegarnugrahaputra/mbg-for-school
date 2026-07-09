using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject levelSelectPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Level Selection")]
    [SerializeField] private Button level1Button;
    [SerializeField] private Button level2Button;
    [SerializeField] private GameObject lockedIconLevel2; // Ikon gembok untuk Level 2

    [Header("Audio Settings")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        // Pengecekan Error: Pastikan tidak ada komponen yang lupa di-drag
        if (mainPanel == null || levelSelectPanel == null || settingsPanel == null)
        {
            Debug.LogWarning("[MainMenuManager] Ada Panel UI yang belum dimasukkan ke Inspector!");
        }

        // 1. Saat pertama mulai, tampilkan Main Panel saja
        ShowMainPanel();

        // 2. Cek apakah Level 2 sudah terbuka (0 = terkunci, 1 = terbuka)
        // Kita pakai PlayerPrefs agar datanya tersimpan walau game ditutup
        int level2Unlocked = PlayerPrefs.GetInt("Level2Unlocked", 0);

        if (level2Unlocked == 1)
        {
            if (level2Button != null) level2Button.interactable = true;
            if (lockedIconLevel2 != null) lockedIconLevel2.SetActive(false);
        }
        else
        {
            if (level2Button != null) level2Button.interactable = false;
            if (lockedIconLevel2 != null) lockedIconLevel2.SetActive(true);
        }

        // 3. Load nilai BGM dan SFX jika sebelumnya sudah di-setting oleh player
        if (bgmSlider != null) bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
        if (sfxSlider != null) sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    // --- FUNGSI UNTUK TOMBOL-TOMBOL UI ---

    public void ShowLevelSelectPanel()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
    }

    public void ShowSettingsPanel()
    {
        mainPanel.SetActive(false);
        levelSelectPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void ShowMainPanel()
    {
        levelSelectPanel.SetActive(false);
        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    // Fungsi ini dipanggil oleh Tombol Level 1 dan Level 2
    public void LoadLevel(string sceneName)
    {
        // Pastikan sceneName sudah dimasukkan di File -> Build Settings
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Player keluar dari Game!");
        Application.Quit();
    }

    // --- FUNGSI UNTUK SETTINGS ---

    public void OnBGMVolumeChanged()
    {
        if (bgmSlider != null)
        {
            PlayerPrefs.SetFloat("BGMVolume", bgmSlider.value);
            // Catatan: Nanti kita hubungkan ke sistem AudioSource
        }
    }

    public void OnSFXVolumeChanged()
    {
        if (sfxSlider != null)
        {
            PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
            // Catatan: Nanti kita hubungkan ke sistem AudioSource
        }
    }
    
    // --- FUNGSI BANTUAN UNTUK TESTING (Bisa dipanggil pakai tombol khusus sementara) ---
    public void DebugResetData()
    {
        PlayerPrefs.DeleteAll(); // Menghapus semua save data
        Debug.Log("Data direset! Level 2 kembali terkunci.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Refresh scene
    }
}
