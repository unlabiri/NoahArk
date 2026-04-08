using UnityEngine;
using Valve.VR.InteractionSystem;

// This gives us a neat dropdown menu in the Inspector to pick colors!
public enum WireColor { Red, Blue, Yellow, Green }

public class VRWirePlug : MonoBehaviour
{
    [Header("Wire Setup")]
    public WireColor myColor;
    public Transform wireBase; // Drag Base_Red here

    private LineRenderer line;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Rigidbody rb;
    private Interactable interactable;

    public bool isPluggedIn = false;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
        interactable = GetComponent<Interactable>();

        // Remember where we started so we can snap back
        startPosition = transform.position;
        startRotation = transform.rotation;
        line.positionCount = 2;
    }

    void Update()
    {
        // Draw the stretchy visual wire every frame
        line.SetPosition(0, wireBase.position);
        line.SetPosition(1, transform.position);

        // Snap back logic: If dropped and NOT plugged in, teleport back to start
        if (!interactable.attachedToHand && !isPluggedIn)
        {
            transform.position = startPosition;
            transform.rotation = startRotation;
            rb.velocity = Vector3.zero; // Stop any physics bouncing
        }
    }

    // The socket calls this when you plug it in correctly
    public void LockIntoSocket(Transform socketTransform)
    {
        isPluggedIn = true;

        // Force the player's SteamVR hand to let go of the wire
        if (interactable.attachedToHand)
        {
            interactable.attachedToHand.DetachObject(gameObject);
        }

        // Snap perfectly into the hole and freeze the physics
        transform.position = socketTransform.position;
        transform.rotation = socketTransform.rotation;
        rb.isKinematic = true;
    }

    // The main box calls this when a random fault happens
    public void EjectWire()
    {
        isPluggedIn = false;
        rb.isKinematic = false; // Unfreeze it so it snaps back to the left side
    }
}