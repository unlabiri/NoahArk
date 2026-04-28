using UnityEngine;
using Valve.VR.InteractionSystem;

public class InvasiveCaptureZone : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AquaInvasionFault    invasionFault;
    [SerializeField] private AquaInvasiveSpawner  spawner;

    [Header("Settings")]
    [SerializeField] private string invasiveTag = "InvasiveSpecies";

    [Header("Cage Visual Indicator")]
    [SerializeField] private Renderer cageIndicatorRenderer;
    [SerializeField] private Color    activeColor   = Color.red;
    [SerializeField] private Color    inactiveColor = Color.white;

    [Header("Feedback")]
    [SerializeField] private ParticleSystem captureParticles;
    [SerializeField] private AudioSource    captureSound;

    private void Update()
    {
        // Cage glows red when there's an active invasion
        if (cageIndicatorRenderer == null || invasionFault == null) return;

        bool active = invasionFault.state == AquaFaultBase.FaultState.Active;
        cageIndicatorRenderer.material.color = active ? activeColor : inactiveColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(invasiveTag)) return;

        var pickup = other.GetComponent<InvasiveSpeciesPickup>();

        // Only count it if the player physically carried it in
        if (pickup == null || !pickup.IsHeld)
        {
            Debug.Log($"{name}: Starfish entered cage zone but wasn't held — ignoring.");
            return;
        }

        if (invasionFault == null)
        {
            Debug.LogWarning($"{name}: No AquaInvasionFault assigned.");
            return;
        }

        if (invasionFault.state != AquaFaultBase.FaultState.Active)
        {
            Debug.Log($"{name}: No active invasion to resolve.");
            return;
        }

        // Cleanly detach from VR hand before disabling
        pickup.ForceDetach();

        spawner?.NotifyCapture(other.gameObject);
        invasionFault.ResolveInvasiveFault();

        if (captureParticles != null) captureParticles.Play();
        if (captureSound     != null) captureSound.Play();

        Debug.Log($"{name}: Invasive species caged successfully!");
        other.gameObject.SetActive(false);
    }
}