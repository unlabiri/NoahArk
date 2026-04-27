using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(Rigidbody))]
public class InvasiveSpeciesPickup : MonoBehaviour
{
    [Header("Haptics")]
    [SerializeField] private float hapticAmplitude = 0.5f;

    [Header("Feedback")]
    [SerializeField] private ParticleSystem grabParticles;
    [SerializeField] private AudioSource grabSound;

    private Rigidbody rb;
    private Hand holdingHand;
    private bool isHeld = false;

    public bool IsHeld => isHeld;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        // Lock in place when starfish spawns — no drifting or falling
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    private void OnAttachedToHand(Hand hand)
    {
        holdingHand = hand;
        isHeld = true;
        rb.isKinematic = true;
        rb.useGravity = false;

        hand.TriggerHapticPulse((ushort)(hapticAmplitude * 3999));
        if (grabParticles != null) grabParticles.Play();
        if (grabSound != null) grabSound.Play();

        Debug.Log($"[InvasivePickup] {name} grabbed by {hand.name}");
    }

    private void OnDetachedFromHand(Hand hand)
    {
        holdingHand = null;
        isHeld = false;
        rb.isKinematic = false;
        rb.useGravity = true;

        rb.velocity = hand.GetTrackedObjectVelocity();
        rb.angularVelocity = hand.GetTrackedObjectAngularVelocity();

        Debug.Log($"[InvasivePickup] {name} released.");
    }

    private void OnHandHoverBegin(Hand hand)
    {
        hand.TriggerHapticPulse(800);
    }

    public void ForceDetach()
    {
        if (holdingHand != null && isHeld)
            holdingHand.DetachObject(gameObject);
    }
}