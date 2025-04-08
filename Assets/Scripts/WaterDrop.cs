using UnityEngine;

public class WaterDrop : MonoBehaviour
{
    private Rigidbody rb;
    private bool launchedForward = false;
    private float thrust = 5f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        if (!launchedForward) {
            rb.AddForce(transform.forward * thrust, ForceMode.Impulse);
            launchedForward = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Water")) {
            Destroy(gameObject);
        }
    }
}
