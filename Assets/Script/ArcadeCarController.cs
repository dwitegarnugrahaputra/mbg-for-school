using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ArcadeCarController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float acceleration = 50f;
    public float turnSpeed = 100f;

    [Header("Engine Audio Settings")]
    public AudioClip engineSoundClip; 
    private AudioSource engineAudioSource;

    private Rigidbody rb;
    private float vertical;
    private float horizontal;

    // Variabel penampung untuk tombol UI
    private bool isGasPressed;
    private bool isBrakePressed;
    private bool isLeftPressed;
    private bool isRightPressed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = 1000f; 
        rb.centerOfMass = new Vector3(0f, -0.5f, 0f);
    }

    private void Start()
    {
        if (engineSoundClip != null)
        {
            engineAudioSource = gameObject.AddComponent<AudioSource>();
            engineAudioSource.clip = engineSoundClip;
            engineAudioSource.loop = true;
            engineAudioSource.playOnAwake = true;
            engineAudioSource.volume = 0.25f;
            engineAudioSource.Play();
        }
    }

    private void Update()
    {
        // Prioritaskan tombol UI, kalau gak dipencet, baru dengerin Keyboard PC
        vertical = Input.GetAxisRaw("Vertical");
        if (isGasPressed) vertical = 1f;
        else if (isBrakePressed) vertical = -1f;

        horizontal = Input.GetAxisRaw("Horizontal");
        if (isRightPressed) horizontal = 1f;
        else if (isLeftPressed) horizontal = -1f;
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(vertical) > 0.1f)
        {
            rb.AddForce(transform.forward * vertical * acceleration, ForceMode.Acceleration);
        }

        if (rb.linearVelocity.magnitude > 0.5f)
        {
            float angle = horizontal * turnSpeed * Time.fixedDeltaTime;
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, angle, 0f));
        }
    }

    private void OnDisable()
    {
        if (engineAudioSource != null)
        {
            engineAudioSource.Stop();
        }
    }

    // --- FUNGSI KHUSUS UNTUK TOMBOL UI ---
    public void PointerDownGas() { isGasPressed = true; }
    public void PointerUpGas() { isGasPressed = false; }

    public void PointerDownBrake() { isBrakePressed = true; }
    public void PointerUpBrake() { isBrakePressed = false; }

    // Lu bisa siapin ini buat tombol Kiri/Kanan yang mau lu desain nanti
    public void PointerDownLeft() { isLeftPressed = true; }
    public void PointerUpLeft() { isLeftPressed = false; }
    
    public void PointerDownRight() { isRightPressed = true; }
    public void PointerUpRight() { isRightPressed = false; }
}