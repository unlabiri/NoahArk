using UnityEngine;
using TMPro; // Needed to talk to the Text screen
using Valve.VR.InteractionSystem; // Needed to read the SteamVR wheel
using UnityEngine.Events;

public class TemperatureFault : MonoBehaviour
{
    [Header("Temperature Status")]
    public bool isBroken = false;
    public float currentTemp = 15f;
    public float safeTemp = 15f;

    [Tooltip("How many degrees the temp goes up per second when broken.")]
    public float heatRiseSpeed = 3f;

    [Header("Hardware References")]
    public TextMeshPro textScreen;
    public LinearMapping valveMapping;

    [Header("Events")]
    public UnityEvent onFaultTriggered;
    public UnityEvent onFaultFixed;

    void Update()
    {
        if (isBroken)
        {
            currentTemp += heatRiseSpeed * Time.deltaTime;

            // UPDATED: Now requires 99% completion to prevent cheating the 2.5 spins!
            if (valveMapping.value >= 0.99f || valveMapping.value <= 0.01f)
            {
                FixFault();
            }
        }

        UpdateScreenVisuals();
    }

    public void TriggerFault()
    {
        if (isBroken) return;

        isBroken = true;
        valveMapping.value = 0.5f; // NEW: Start the wheel perfectly in the middle

        Debug.Log("AC Fault! Temperature rising!");
        onFaultTriggered.Invoke();
    }

    private void FixFault()
    {
        isBroken = false;
        currentTemp = safeTemp; // Instantly drop the temp back to normal
        Debug.Log("AC Repaired! Coolant flushed.");
        onFaultFixed.Invoke();
    }

    private void UpdateScreenVisuals()
    {
        // Update the text to show the exact whole number
        textScreen.text = Mathf.RoundToInt(currentTemp).ToString() + "°F";

        // Change screen color to red if it gets above freezing (32 F)
        if (currentTemp > 32f)
        {
            textScreen.color = Color.red;
        }
        else
        {
            textScreen.color = Color.cyan;
        }
    }
}