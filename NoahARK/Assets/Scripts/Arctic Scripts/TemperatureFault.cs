using UnityEngine;
using TMPro;
using Valve.VR.InteractionSystem;
using UnityEngine.Events;

public class TemperatureFault : MonoBehaviour
{
    [Header("Temperature Status")]
    public bool isBroken = false;
    public float currentTemp = 15f;
    public float safeTemp = 15f;
    public ArcticBiomeController arcticController;

    [Tooltip("How many degrees the temp goes up per second when broken.")]
    public float heatRiseSpeed = 3f;

    [Header("Hardware References")]
    public TextMeshPro textScreen;
    public LinearMapping valveMapping;

    [Header("Valve Settings (NEW)")]
    [Tooltip("How much total movement is needed to fix the pipe. 1.0 = one full mapping traversal.")]
    public float requiredSpinAmount = 1.0f;
    private float lastValveValue;
    private float accumulatedSpin = 0f;

    [Header("Vent Integration")]
    public SimpleDig ventFault;

    [Header("Events")]
    public UnityEvent onFaultTriggered;
    public UnityEvent onFaultFixed;

    void Update()
    {
        bool isVentBlocked = (ventFault != null && ventFault.isBroken);
        float currentSpeed = 0;

        if (isBroken || isVentBlocked)
        {
            currentSpeed = (isBroken && isVentBlocked) ? (heatRiseSpeed * 2) : heatRiseSpeed;
            currentTemp += currentSpeed * Time.deltaTime;
        }

        if (isBroken || isVentBlocked)
        {
            currentTemp += currentSpeed * Time.deltaTime;
            arcticController.UpdateTemperature(currentTemp);
        }


        // --- THE NEW SPIN LOGIC ---
        if (isBroken)
        {
            // 1. Calculate how much the wheel moved this frame (Absolute value ignores left/right)
            float spinDelta = Mathf.Abs(valveMapping.value - lastValveValue);

            // 2. Add that movement to our total
            accumulatedSpin += spinDelta;

            // 3. Save current position for the next frame
            lastValveValue = valveMapping.value;

            // 4. Did they spin it enough?
            if (accumulatedSpin >= requiredSpinAmount)
            {
                FixPipeFault();
            }
        }

        UpdateScreenVisuals();
    }

    public void TriggerFault()
    {
        if (isBroken) return;

        isBroken = true;

        // Reset our spin counters to zero
        accumulatedSpin = 0f;
        lastValveValue = valveMapping.value; // Start tracking from wherever it's currently resting

        Debug.Log("AC Fault! Coolant Pipe leaking. Temperature rising!");
        onFaultTriggered.Invoke();
    }

    private void FixPipeFault()
    {
        isBroken = false;
        Debug.Log("AC Repaired! Coolant flushed.");

        CheckIfFullySafe();
        onFaultFixed.Invoke();
    }

    public void VentFixedFromExternal()
    {
        Debug.Log("Air Vent cleared of snow!");
        CheckIfFullySafe();
    }

    private void CheckIfFullySafe()
    {
        bool isVentBlocked = (ventFault != null && ventFault.isBroken);

        if (!isBroken && !isVentBlocked)
        {
            currentTemp = safeTemp;
            Debug.Log("All AC systems nominal. Temperature restored.");
        }
    }

    private void UpdateScreenVisuals()
    {
        textScreen.text = Mathf.RoundToInt(currentTemp).ToString() + "°F";

        if (currentTemp > 32f)
        {
            textScreen.color = Color.yellow;
        }
        else
        {
            textScreen.color = Color.red;
        }
    }
}