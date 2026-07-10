using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio; // JANGAN LUPA INI: Wajib ditambahkan untuk memanipulasi Audio Mixer!

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
    [SerializeField] private AudioMixer mainMixer; // Tambahan: Tempat nge-drag MainMixer lu
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
        // Default kita ganti ke 0.75f (75%) biar tidak terlalu memekakkan telinga di awal
        if (bgmSlider != null)
        {
            bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0.75f);
            ApplyBGMVolume(bgmSlider.value); // Langsung tembakkan volume ke mixer saat start
        }
        
        if (sfxSlider != null)
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
            // Catatan: Jika nanti lu bikin grup SFX di mixer, panggil fungsi apply-nya di sini
        }
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

    public void LoadLevel(string sceneName)
    {
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
            ApplyBGMVolume(bgmSlider.value); // Panggil fungsi pembantu untuk mengubah volume asli di mixer
        }
    }

    public void OnSFXVolumeChanged()
    {
        if (sfxSlider != null)
        {
            PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
            // Catatan: Tempat menyambungkan volume SFX ke mixer nanti jika diperlukan
        }
    }

    // --- FUNGSI PEMBANTU (AUDIO MATHEMATICS) ---
    private void ApplyBGMVolume(float value)
    {
        if (mainMixer != null)
        {
            // Mengubah nilai linear slider (0 sampai 1) menjadi skala desibel logaritmik (-80dB sampai 20dB)
            float dbValue = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20;
            mainMixer.SetFloat("BGMVolume", dbValue); // "BGMVolume" harus sama persis dengan nama parameter exposed lu!
        }
    }
    
    // --- FUNGSI BANTUAN UNTUK TESTING ---
    public void DebugResetData()
    {
        PlayerPrefs.DeleteAll(); 
        Debug.Log("Data direset! Level 2 kembali terkunci.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }
}