using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public AudioSource audioTabrakan;
    // Wadah buat masukin Panel Pause dari Unity
    public GameObject panelPause;

    // Fungsi buat nge-pause game
    public void PauseGame()
    {
        panelPause.SetActive(true);    // Munculin panel pause
        Time.timeScale = 0f;          // Hentikan waktu game (freeze)
    }

    // Fungsi buat lanjutin game lagi
    public void ResumeGame()
    {
        panelPause.SetActive(false);   // Sembunyikan panel pause
        Time.timeScale = 1f;          // Jalankan lagi waktu game
    }

    // Fungsi buat ulang game dari awal level ini
    public void RetryGame()
    {
        Time.timeScale = 1f;          // Pastikan waktu jalan lagi sebelum reload
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Fungsi buat kembali ke Main Menu
    public void BackToMainMenu()
    {
        Time.timeScale = 1f;          // Pastikan waktu jalan lagi sebelum pindah scene
        SceneManager.LoadScene("MainMenu"); // Sesuaikan dengan nama scene Main Menu lu
    }
}