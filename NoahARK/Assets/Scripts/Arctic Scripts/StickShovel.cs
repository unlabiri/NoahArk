using UnityEngine;

public class StickyShovel : MonoBehaviour
{
    private Rigidbody rb;
    public bool canStick = true; // Changed to public so you can see it in the Inspector!

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ShovelGrabbed()
    {
        canStick = false;
    }

    public void ShovelDropped()
    {
        canStick = true;
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        // THE SPY LOG: This prints every single time the shovel hits ANYTHING.
        Debug.Log($"Shovel hit: {collision.gameObject.name} | Tag: {collision.gameObject.tag} | canStick is: {canStick}");

        if (!canStick) return;

        if (collision.gameObject.CompareTag("Floor"))
        {
            // I removed the downward velocity check. If it hits the floor, it sticks. Period.
            rb.isKinematic = true;
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

            Debug.Log("<color=cyan>[StickyShovel]</color> SUCCESS! Shovel stuck!");
        }
    }
}