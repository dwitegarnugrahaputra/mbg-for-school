using UnityEngine;

public class VehicleHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [Header("Damage Settings")]
    [SerializeField] private float minImpactForce = 4f; // Batas minimum kecepatan tabrakan agar dapet damage
    [SerializeField] private float damageMultiplier = 2.5f; // Faktor pengali damage

    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;

        // Daftarkan ke GameManager jika ada di Scene
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateHealthUI(currentHealth, maxHealth);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        // Mengukur seberapa keras hantaman berdasarkan kecepatan relatif fisika Rigidbody
        float impactForce = collision.relativeVelocity.magnitude;

        // Hanya beri damage kalau tabrakan cukup kencang (biar senggolan tipis/gesekan gak ngurangin darah)
        if (impactForce >= minImpactForce)
        {
            float damageAmount = impactForce * damageMultiplier;
            TakeDamage(damageAmount);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        Debug.Log($"[SPPG LOGISTIC] Truk lecet! Sisa Health: {currentHealth}");

        // Kirim data health terbaru ke GameManager untuk diupdate ke UI
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateHealthUI(currentHealth, maxHealth);
        }

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("<color=red>[BREAKING NEWS] Mobil logistik MBG hancur karena ugal-ugalan!</color>");
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TriggerGameOver();
        }
    }
}