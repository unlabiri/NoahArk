using UnityEngine;
using Valve.VR.InteractionSystem;   // SteamVR Interaction System

[RequireComponent(typeof(Interactable))]
public class LeverInteractable : MonoBehaviour
{
    [Header("Hinge Settings")]
    public HingeJoint hinge;          // The HingeJoint on this lever
    public float minAngle = -45f;
    public float maxAngle =  45f;

    [Header("Lever Pivot")]
    // The axis the lever rotates around in WORLD space.
    // If your lever tilts forward/back use Vector3.right,
    // side-to-side use Vector3.forward.
    public Vector3 hingeWorldAxis = Vector3.right;

    // The pivot point the lever rotates around (usually this transform's position)
    public Transform pivotPoint;

    // -------------------------------------------------------
    // Private state
    // -------------------------------------------------------
    private Hand holdingHand = null;
    private Interactable interactable;

    void Awake()
    {
        interactable = GetComponent<Interactable>();
        if (pivotPoint == null) pivotPoint = transform;

        // Make sure hinge limits match your settings
        if (hinge != null)
        {
            JointLimits limits = hinge.limits;
            limits.min = minAngle;
            limits.max = maxAngle;
            hinge.limits = limits;
            hinge.useLimits = true;

            // Disable the hinge motor/spring so physics doesn't fight us
            hinge.useMotor  = false;
            hinge.useSpring = false;
        }
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
        Vector3 toHand = hand.transform.position - pivotPoint.position;

        // Remove the component along the hinge axis so we stay in the rotation plane
        Vector3 axisNorm = hingeWorldAxis.normalized;
        Vector3 projected = toHand - Vector3.Dot(toHand, axisNorm) * axisNorm;

        if (projected.sqrMagnitude < 0.0001f) return;

        // Reference direction = lever at angle 0 (straight down or forward)
        // Adjust referenceDir to match your lever's neutral pose
        Vector3 referenceDir = Vector3.down; // Change to Vector3.forward if needed

        float angle = Vector3.SignedAngle(referenceDir, projected.normalized, axisNorm);
        angle = Mathf.Clamp(angle, minAngle, maxAngle);

        // Apply rotation: keep world position, only change rotation around hinge axis
        transform.localRotation = Quaternion.AngleAxis(angle, transform.InverseTransformDirection(axisNorm));
    }

    // -------------------------------------------------------
    // Optional: freeze lever in place when released
    // (remove if you want it to fall back by gravity)
    // -------------------------------------------------------
    private void OnDetachedFromHand_Post(Hand hand)
    {
        if (hinge != null)
        {
            // Lock in place by briefly enabling a spring at current angle
            JointSpring spring = hinge.spring;
            spring.targetPosition = hinge.angle;
            spring.spring = 500f;
            spring.damper = 50f;
            hinge.spring = spring;
            hinge.useSpring = true;
        }
    }
}
