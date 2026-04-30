using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]

public class LeverInteractable : MonoBehaviour
{
    [Header("Hinge Settings")]
    public Vector3 localRotationAxis = Vector3.right;
    public float minAngle = -45f;
    public float maxAngle = 45f;
    public float followSpeed = 20f;

    private Hand holdingHand = null;
    private float currentAngle = 0f;
    private Quaternion neutralRotation;
    private Rigidbody rb;

    public bool IsBeingHeld => holdingHand != null;

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

    [Header("Attachment")]
    public Transform attachmentPoint; // drag your AttachmentPoint here in Inspector

    // Hand hovers over lever - wait for trigger press
    private void HandHoverUpdate(Hand hand)
    {
        GrabTypes grabType = hand.GetGrabStarting();

        if (grabType != GrabTypes.None)
        {
            hand.AttachObject(
                gameObject,
                grabType,
                Hand.AttachmentFlags.TurnOnKinematic |
                Hand.AttachmentFlags.DetachFromOtherHand,
                attachmentPoint);  // ? pass the transform directly
        }
    }


    // Called when hand successfully attaches
    private void OnAttachedToHand(Hand hand)
    {
        holdingHand = hand;
    }

    // Called every frame while attached
    private void HandAttachedUpdate(Hand hand)
    {
        // Release when trigger is let go
        if (hand.IsGrabEnding(gameObject))
        {
            hand.DetachObject(gameObject);
            return;
        }

        if (hand == null) return;

        Vector3 worldAxis = transform.parent != null
            ? transform.parent.TransformDirection(localRotationAxis.normalized)
            : localRotationAxis.normalized;

        Vector3 toHand = hand.transform.position - transform.position;
        Vector3 flatHand = toHand - Vector3.Dot(toHand, worldAxis) * worldAxis;
        if (flatHand.sqrMagnitude < 0.0001f) return;

        Vector3 neutralFwd = neutralRotation * Vector3.forward;
        Vector3 flatFwd = neutralFwd - Vector3.Dot(neutralFwd, worldAxis) * worldAxis;
        if (flatFwd.sqrMagnitude < 0.0001f) return;

        float targetAngle = Vector3.SignedAngle(
            flatFwd.normalized,
            flatHand.normalized,
            worldAxis);

        targetAngle = Mathf.Clamp(targetAngle, minAngle, maxAngle);
        currentAngle = Mathf.MoveTowards(currentAngle, targetAngle, followSpeed * Time.deltaTime * 90f);

        Quaternion targetRot = Quaternion.AngleAxis(currentAngle, worldAxis) * neutralRotation;
        rb.MoveRotation(targetRot);
    }

    private void OnDetachedFromHand(Hand hand)
    {
        holdingHand = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 axis = transform.TransformDirection(localRotationAxis.normalized);
        Gizmos.DrawRay(transform.position, axis * 0.3f);
        Gizmos.DrawRay(transform.position, -axis * 0.3f);
    }
}