using UnityEngine;
using System.Collections;

public class DeliveryZone : MonoBehaviour
{
    [Header("Zone Settings")]
    [SerializeField] private string schoolName = "Sekolah 1";
    [SerializeField] private float deliveryDuration = 2f; // Waktu loading 2 detik

    [Header("Visual References")]
    [SerializeField] private Renderer zoneRenderer; // Komponen Mesh Renderer kotak merah
    [SerializeField] private Color completedColor = Color.green; // Warna hijau setelah sukses

    private bool isDelivering = false;
    private bool isMissionCompleted = false;

    private void OnTriggerEnter(Collider other)
    {
        // Cek jika yang masuk adalah Mobil Van Tegar ("Player")
        if (other.CompareTag("Player") && !isMissionCompleted && !isDelivering)
        {
            // Mulai proses loading bongkar muat menggunakan Coroutine
            StartCoroutine(StartDeliveryLoading());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Fitur Tambahan: Kalau mobil keluar sebelum 2 detik selesai, loading dibatalkan
        if (other.CompareTag("Player") && isDelivering && !isMissionCompleted)
        {
            StopAllCoroutines();
            isDelivering = false;
            Debug.Log("[SPPG LOGISTIC] Pengiriman gagal! Mobil keluar dari zona sebelum loading selesai.");
        }
    }

    private IEnumerator StartDeliveryLoading()
    {
        isDelivering = true;
        Debug.Log($"[SPPG LOGISTIC] Sedang membongkar muatan di {schoolName}... Harap tunggu {deliveryDuration} detik.");

        // Menunggu selama waktu yang ditentukan (2 detik)
        yield return new WaitForSeconds(deliveryDuration);

        CompleteDelivery();
    }

    private void CompleteDelivery()
    {
        isDelivering = false;
        isMissionCompleted = true;

        // 1. Mengubah warna zona dari merah menjadi hijau
        if (zoneRenderer != null)
        {
            // Mengubah warna utama material objek secara real-time
            zoneRenderer.material.color = completedColor;
        }

        // 2. Memunculkan notifikasi satir/lokal sukses di Console
        Debug.Log($"<color=green>[NOTIFIKASI] Ompreng MBG berhasil terkirim ke {schoolName}!</color>");
    }
}