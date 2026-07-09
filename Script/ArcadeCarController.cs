using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ArcadeCarController : MonoBehaviour
{
    [Header("Movement")]
    public float acceleration = 20f;
    public float maxSpeed = 20f;
    public float turnSpeed = 90f;

    private Rigidbody rb;

    private float vertical;
    private float horizontal;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.mass = 1000f;
        rb.centerOfMass = new Vector3(0f, -0.5f, 0f);
    }

    private void Update()
    {
        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        // GAS
        if (Mathf.Abs(vertical) > 0.01f)
        {
            rb.AddForce(transform.forward * vertical * acceleration, ForceMode.Acceleration);
        }

        // BATASI KECEPATAN
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        if (flatVelocity.magnitude > maxSpeed)
        {
            flatVelocity = flatVelocity.normalized * maxSpeed;

            rb.linearVelocity = new Vector3(
                flatVelocity.x,
                rb.linearVelocity.y,
                flatVelocity.z
            );
        }

        // BELOK
        if (rb.linearVelocity.magnitude > 0.2f)
        {
            float angle = horizontal * turnSpeed * Time.fixedDeltaTime;

            rb.MoveRotation(
                rb.rotation * Quaternion.Euler(0, angle, 0)
            );
        }
    }
}