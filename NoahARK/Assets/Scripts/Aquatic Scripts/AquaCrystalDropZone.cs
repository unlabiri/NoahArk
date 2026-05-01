using UnityEngine;

public class AquaCrystalDropZone : MonoBehaviour
{
    [SerializeField] private AquaTemperatureFault temperatureFault;
    [SerializeField] private AquaticBiomeController biomeController;
    [SerializeField] private float resetTemperature = 75f;

    private void OnTriggerEnter(Collider other)
    {
        AquaCrystal crystal = other.GetComponent<AquaCrystal>();
        if (crystal == null) return;

        temperatureFault?.ResolveWithCrystal();

        if (biomeController != null)
            biomeController.State.temperature = resetTemperature;

        Debug.Log("[CrystalDropZone] Crystal consumed — temperature reset.");
        crystal.Consume();
    }
}