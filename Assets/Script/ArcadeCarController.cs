using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ArcadeCarController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float maxSpeed = 20f;       // Top speed mobil
    [SerializeField] private float acceleration = 10f;   // Seberapa cepat dia mencapai top speed
    [SerializeField] private float turnSpeed = 90f;      // Kecepatan belok

    private Rigidbody rb;
    private float currentSpeed = 0f;
    private float moveInput;
    private float steerInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Memastikan mobil pakai gravitasi dan gak guling-guling
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void Update()
    {
        // Membaca tombol W/S dan A/D
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        // 1. LOGIKA AKSELERASI MULUS (PASTI JALAN)
        // Menghitung target kecepatan berdasarkan tombol W atau S
        float targetSpeed = moveInput * maxSpeed;
        
        // Memaksa kecepatan naik secara bertahap dari lambat ke cepat (Anti-Mogok)
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);

        // Menimpa kecepatan gerak mobil secara paksa ke arah depan, tapi membiarkan Y tetap untuk gravitasi
        Vector3 newVelocity = transform.forward * currentSpeed;
        newVelocity.y = rb.linearVelocity.y; 
        rb.linearVelocity = newVelocity;

        // 2. LOGIKA BELOK ARCADE
        // Mobil cuma bisa belok kalau lagi punya kecepatan
        if (Mathf.Abs(currentSpeed) > 0.1f)
        {
            float direction = Mathf.Sign(currentSpeed); // Biar kalau mundur, setirnya kebalik natural
            float turnAmount = steerInput * turnSpeed * direction * Time.fixedDeltaTime;
            
            // Memutar badan mobil langsung
            transform.Rotate(0, turnAmount, 0);
        }
    }
}