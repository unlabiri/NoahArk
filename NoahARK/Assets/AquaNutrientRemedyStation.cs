using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class AquaNutrientRemedyStation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AquaNutrientFault nutrientFault;

    [Header("Valve Settings")]
    [SerializeField] private float totalRotationRequired = 360f;
    [SerializeField] private float rotationSpeed = 90f;

    [Header("Feedback")]
    [SerializeField] private AudioSource crankSound;
    [SerializeField] private AudioSource dingSound;

    [Header("Debug — no headset")]
    [SerializeField] private KeyCode testKey = KeyCode.F;

    private float currentRotation = 0f;
    private bool isBeingTurned = false;
    private bool isSolved = false;

    private void Update()
    {
        if (isSolved) return;

        // Fixed: use GetKeyDown/GetKeyUp so it doesn't get stuck on
        if (Input.GetKeyDown(testKey))
            isBeingTurned = true;
        else if (Input.GetKeyUp(testKey))
            isBeingTurned = false;

        if (isBeingTurned)
        {
            float step = rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up, step, Space.Self);
            currentRotation += step;

            if (crankSound != null && !crankSound.isPlaying)
                crankSound.Play();

            if (currentRotation >= totalRotationRequired)
                Solve();
        }
        else
        {
            if (crankSound != null && crankSound.isPlaying)
                crankSound.Stop();
        }
    }

    // ── SteamVR hand grip ─────────────────────────────────────────────────
    private void HandHoverUpdate(Hand hand)
    {
        if (isSolved) return;

        if (hand.GetGrabStarting() != GrabTypes.None)
        {
            isBeingTurned = true;
            hand.TriggerHapticPulse(1000);
        }

        if (hand.GetGrabEnding() != GrabTypes.None)
            isBeingTurned = false;
    }

    private void OnHandHoverEnd(Hand hand)
    {
        isBeingTurned = false;
    }

    // ── Solve ─────────────────────────────────────────────────────────────
    private void Solve()
    {
        isSolved = true;
        isBeingTurned = false;

        if (crankSound != null) crankSound.Stop();
        if (dingSound != null) dingSound.Play();

        if (nutrientFault != null)
        {
            nutrientFault.AddNutrients(1f);
            Debug.Log("[NutrientRemedy] Valve turned — nutrient fault resolved!");
        }
        else
        {
            Debug.LogWarning("[NutrientRemedy] nutrientFault reference is missing!");
        }
    }

    public void ResetValve()
    {
        isSolved = false;
        isBeingTurned = false;
        currentRotation = 0f;
        transform.localRotation = Quaternion.identity;
    }
}