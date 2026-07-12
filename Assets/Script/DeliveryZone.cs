using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    [Header("Zone Settings")]
    [SerializeField] private float requiredStopTime = 3f;
    private float currentStopTime = 0f;
    private bool isDelivered = false;

    [SerializeField] private Renderer zoneRenderer;
    [SerializeField] private Color successColor = Color.green;

    [Header("System Link")]
    [SerializeField] private DeliveryManager deliveryManager; // Colokan ke Otak Utama

    private void Start()
    {
        if (zoneRenderer == null) zoneRenderer = GetComponent<Renderer>();
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
                    // Berubah hijau
                    if (zoneRenderer != null) zoneRenderer.material.color = successColor;
                    
                    // Lapor ke otak utama
                    if (deliveryManager != null) deliveryManager.RecordDelivery(other.gameObject);
                    
                    isDelivered = true; // Kunci area ini biar nggak ke-trigger 2x
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
}