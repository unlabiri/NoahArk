using UnityEngine;

public class IglooMelter : MonoBehaviour
{
    [Header("Ship Systems")]
    [Tooltip("Connect the Coolant Pipe to monitor hull temperature.")]
    public TemperatureFault shipTempControl;

    [Header("Phase Change Thresholds")]
    public float slushThreshold = 32f;    // When the ice starts to soften
    public float collapseThreshold = 50f; // When the structure fails

    [Header("Igloo Physical States")]
    public GameObject solidStructure;      // The healthy igloo
    public GameObject slumpingStructure;   // The slightly melted version
    public GameObject puddleStructure;     // The critical/melted version

    // Internal tracker for the igloo's current physical condition
    private int currentCondition = -1;

    void Update()
    {
        if (shipTempControl == null) return;

        float internalTemp = shipTempControl.currentTemp;
        int desiredCondition = 0; // Default: Solid

        // Logic check: How bad is the heat?
        if (internalTemp >= collapseThreshold)
        {
            desiredCondition = 2; // Puddle
        }
        else if (internalTemp >= slushThreshold)
        {
            desiredCondition = 1; // Slumping
        }

        // Only swap if the condition has changed to save performance
        if (desiredCondition != currentCondition)
        {
            UpdateIglooPhysicality(desiredCondition);
        }
    }

    private void UpdateIglooPhysicality(int newCondition)
    {
        currentCondition = newCondition;

        // Step 1: Deactivate all versions to clear the space
        if (solidStructure != null) solidStructure.SetActive(false);
        if (slumpingStructure != null) slumpingStructure.SetActive(false);
        if (puddleStructure != null) puddleStructure.SetActive(false);

        // Step 2: Activate the version that matches the temperature
        if (newCondition == 0 && solidStructure != null)
        {
            solidStructure.SetActive(true);
        }
        else if (newCondition == 1 && slumpingStructure != null)
        {
            slumpingStructure.SetActive(true);
        }
        else if (newCondition == 2 && puddleStructure != null)
        {
            puddleStructure.SetActive(true);
        }
    }
}