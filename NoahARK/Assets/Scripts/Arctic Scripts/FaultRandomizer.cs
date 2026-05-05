using System.Collections;
using UnityEngine;

public class FaultRandomizer : MonoBehaviour
{
    [Header("Timing Settings")]
    [Tooltip("Time in seconds before the first fault is allowed to happen. (60 = 1 minute)")]
    public float initialGracePeriod = 60f;

    [Tooltip("Minimum time in seconds between faults. (120 = 2 minutes)")]
    public float minTimeBetweenFaults = 120f;

    [Tooltip("Maximum time in seconds between faults. (240 = 4 minutes)")]
    public float maxTimeBetweenFaults = 240f;

    [Header("The Ship Faults")]
    public WiringBox wiringBox; // Make sure this matches your script name!
    public TemperatureFault tempPipe;
    public SimpleDig snowDig;

    private void Start()
    {
        // Start the background timer the moment the game loads
        StartCoroutine(FaultTimerRoutine());
    }

    private IEnumerator FaultTimerRoutine()
    {
        // 1. Wait out the initial 1-minute safe zone
        Debug.Log("Game started. 60-second grace period active.");
        yield return new WaitForSeconds(initialGracePeriod);

        // 2. Loop this section forever
        while (true)
        {
            // Pick a random time between 2 and 4 minutes (Averages out to 3 minutes)
            float waitTime = Random.Range(minTimeBetweenFaults, maxTimeBetweenFaults);

            Debug.Log($"Next fault will occur in {waitTime} seconds.");
            yield return new WaitForSeconds(waitTime);

            // Time is up! Trigger exactly one fault.
            TriggerRandomFault();
        }
    }

    private void TriggerRandomFault()
    {
        // Pick a random number: 0, 1, or 2
        int randomFault = Random.Range(0, 3);

        switch (randomFault)
        {
            case 0:
                if (wiringBox != null) wiringBox.TriggerFault();
                Debug.Log("Randomizer chose: Wiring Box");
                break;
            case 1:
                if (tempPipe != null) tempPipe.TriggerFault();
                Debug.Log("Randomizer chose: Temp Pipe");
                break;
            case 2:
                if (snowDig != null) snowDig.TriggerFault();
                Debug.Log("Randomizer chose: Snow Dig");
                break;
        }
    }
}