using UnityEngine;

public class AquaCrystalDropZone : MonoBehaviour
{
    [SerializeField] private AquaTemperatureFault temperatureFault;
    [SerializeField] private AquaticBiomeController biomeController;
    [SerializeField] private float resetTemperature = 75f;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[CrystalDropZone] Something entered: {other.gameObject.name}");

        AquaCrystal crystal = other.GetComponent<AquaCrystal>();
        if (crystal == null)
        {
            Debug.LogWarning("[CrystalDropZone] No AquaCrystal component found on: " + other.gameObject.name);
            return;
        }

        temperatureFault?.ResolveWithCrystal();

        if (biomeController != null)
            biomeController.State.temperature = resetTemperature;

        Debug.Log("[CrystalDropZone] Crystal consumed — temperature reset.");
        crystal.Consume();
    }
}