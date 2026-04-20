using UnityEngine;

public class EnvironmentalMelter : MonoBehaviour
{
    [Header("The AC System")]
    [Tooltip("Drag your Coolant Pipe here so it knows the current temp.")]
    public TemperatureFault tempSystem;

    [Header("Melting Thresholds")]
    public float slightMeltTemp = 32f; // Freezing point
    public float criticalMeltTemp = 50f; // Dangerously hot

    [Header("The 3D Models")]
    public GameObject healthyModel;
    public GameObject slightlyMeltedModel;
    public GameObject criticallyMeltedModel;

    // We track the state so we don't unnecessarily swap models every single frame
    private int currentState = -1;

    void Update()
    {
        if (tempSystem == null) return;

        float currentTemp = tempSystem.currentTemp;
        int targetState = 0; // Default to Healthy (0)

        // Determine which state we SHOULD be in based on the temperature
        if (currentTemp >= criticalMeltTemp)
        {
            targetState = 2; // Critical
        }
        else if (currentTemp >= slightMeltTemp)
        {
            targetState = 1; // Slight Melt
        }

        // If the state needs to change, swap the 3D models!
        if (targetState != currentState)
        {
            SwapModels(targetState);
        }
    }

    private void SwapModels(int newState)
    {
        currentState = newState;

        // 1. Turn every model OFF
        if (healthyModel != null) healthyModel.SetActive(false);
        if (slightlyMeltedModel != null) slightlyMeltedModel.SetActive(false);
        if (criticallyMeltedModel != null) criticallyMeltedModel.SetActive(false);

        // 2. Turn only the correct model ON
        if (newState == 0 && healthyModel != null)
            healthyModel.SetActive(true);
        else if (newState == 1 && slightlyMeltedModel != null)
            slightlyMeltedModel.SetActive(true);
        else if (newState == 2 && criticallyMeltedModel != null)
            criticallyMeltedModel.SetActive(true);
    }
}