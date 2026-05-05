using UnityEngine;
using TMPro;

public class AquaTempController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AquaticBiomeController biomeController;
    [SerializeField] private AquaTemperatureFault temperatureFault;

    [Header("Display")]
    [SerializeField] private TMP_Text temperatureText;

    [Header("Cooling Settings")]
    [SerializeField] private int coolAmount = 1;

    private AquaticBiomeState biomeState;

    private void Start()
    {
        biomeState = biomeController.State;
    }

    private void Update()
    {
        if (biomeState != null && temperatureText != null)
            temperatureText.text = biomeState.temperature.ToString("F1") + "°F";
    }

    public void CoolWater()
    {
        if (temperatureFault == null)
        {
            Debug.LogWarning("[AquaTempController] No temperatureFault assigned!");
            return;
        }

        temperatureFault.CoolWater(coolAmount);
        Debug.Log($"[AquaTempController] Cooled water by {coolAmount}");
    }
}