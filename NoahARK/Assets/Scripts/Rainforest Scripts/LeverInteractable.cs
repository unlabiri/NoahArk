using UnityEngine;
using Valve.VR.InteractionSystem;   // SteamVR Interaction System

[RequireComponent(typeof(Interactable))]
public class LeverInteractable : MonoBehaviour
{
    [Header("Hinge Settings")]
    public Vector3 localRotationAxis;
    public float minAngle = -45f;
    public float maxAngle =  45f;

    [Header("Lever Pivot")]
    // The axis the lever rotates around in WORLD space.
    // If your lever tilts forward/back use Vector3.right,
    // side-to-side use Vector3.forward.


    // The pivot point the lever rotates around (usually this transform's position)
    public float followSpeed = 20f;

    // -------------------------------------------------------
    // Private state
    // -------------------------------------------------------
    private Hand holdingHand = null;
    private Interactable interactable;
    private float currentAngle = 0f;
    private Quaternion neutralRotation;
    private Rigidbody rb;

    public float NormalizedValue =>
        Mathf.InverseLerp(minAngle, maxAngle, currentAngle);

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        neutralRotation = transform.rotation;
    }

    // -------------------------------------------------------
    // SteamVR calls these automatically via the Interactable
    // -------------------------------------------------------
    private void OnAttachedToHand(Hand hand)
    {
        holdingHand = hand;
    }

    private void OnDetachedFromHand(Hand hand)
    {
        holdingHand = null;
    }

    // -------------------------------------------------------
    // While held, project the hand's position onto the hinge
    // arc and rotate the lever accordingly
    // -------------------------------------------------------
    private void HandAttachedUpdate(Hand hand)
    {
        if (hand == null) return;

        // Vector from pivot to hand
        Vector3 worldAxis = transform.parent != null ? transform.parent.TransformDirection(localRotationAxis.normalized)
            : localRotationAxis.normalized;
        Vector3 toHand = hand.transform.position - transform.position;
        Vector3 flatHand = toHand - Vector3.Dot(toHand, worldAxis) * worldAxis;

        if (flatHand.sqrMagnitude < 0.0001f) return;

        Vector3 neutralFwd = neutralRotation * Vector3.forward;
        Vector3 flatFwd = neutralFwd - Vector3.Dot(neutralFwd, worldAxis) * worldAxis;

        if (flatFwd.sqrMagnitude < .0001f) return;

        float targetAngle = Vector3.SignedAngle(
            flatFwd.normalized,
            flatHand.normalized,
            worldAxis
            );
        targetAngle = Mathf.Clamp(targetAngle, minAngle, maxAngle);

        currentAngle = Mathf.MoveTowards(currentAngle, targetAngle, followSpeed * Time.deltaTime * 90f);

        Quaternion targetRot = Quaternion.AngleAxis(currentAngle, worldAxis) * neutralRotation;

        rb.MoveRotation(targetRot);
    }

    // -------------------------------------------------------
    // Optional: freeze lever in place when released
    // (remove if you want it to fall back by gravity)
    // -------------------------------------------------------
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 axis = transform.TransformDirection(localRotationAxis.normalized);
        Gizmos.DrawRay(transform.position, axis * 0.3f);
        Gizmos.DrawRay(transform.position, -axis * 0.3f);
    }
}
