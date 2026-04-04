using UnityEngine;

public class InvasiveCaptureZone : MonoBehaviour
{
    [SerializeField] private AquaInvasionFault invasionFault;
    [SerializeField] private string invasiveTag = "InvasiveSpecies";
    [SerializeField] private bool disableCapturedObject = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(invasiveTag))
            return;

        if (invasionFault == null)
        {
            Debug.LogWarning($"{name}: No AquaInvasionFault assigned.");
            return;
        }

        if (invasionFault.state != AquaFaultBase.FaultState.Active)
        {
            Debug.Log($"{name}: Invasive object entered cage, but invasion fault is not active.");
            return;
        }

        invasionFault.ResolveInvasiveFault();

        Debug.Log($"{name}: Invasive species captured.");

        if (disableCapturedObject)
        {
            other.gameObject.SetActive(false);
        }
    }
}