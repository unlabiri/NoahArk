using System.Collections.Generic;
using UnityEngine;

public class AquaRandomizer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WakeCycleManager wakeCycle;

    [SerializeField] private AquaInvasionFault invasionFault;

    // Placeholder references for later
    [SerializeField] private AquaFaultBase nutrientFault;
    [SerializeField] private AquaFaultBase temperatureFault;

    [Header("Rules")]
    [Range(0f, 1f)]
    [SerializeField] private float secondFaultChance = 0.5f;

    private void OnEnable()
    {
        if (wakeCycle == null)
        {
            Debug.LogError($"{name}: WakeCycleManager is missing.");
            enabled = false;
            return;
        }

        wakeCycle.OnYearChange += HandleNewDay;
    }

    private void OnDisable()
    {
        if (wakeCycle != null)
            wakeCycle.OnYearChange -= HandleNewDay;
    }

    private void HandleNewDay(int dayCount)
    {
        List<AquaFaultBase> availableFaults = GetAvailableFaults();

        if (availableFaults.Count == 0)
        {
            Debug.Log($"Day {dayCount}: No available faults to activate.");
            return;
        }

        // Always activate at least one fault
        AquaFaultBase firstFault = PickRandomFault(availableFaults);
        firstFault.Activate();
        availableFaults.Remove(firstFault);

        Debug.Log($"Day {dayCount}: Activated first fault = {firstFault.name}");

        // Maybe activate a second fault
        if (availableFaults.Count > 0 && Random.value < secondFaultChance)
        {
            AquaFaultBase secondFault = PickRandomFault(availableFaults);
            secondFault.Activate();

            Debug.Log($"Day {dayCount}: Activated second fault = {secondFault.name}");
        }
    }

    private List<AquaFaultBase> GetAvailableFaults()
    {
        List<AquaFaultBase> available = new List<AquaFaultBase>();

        if (invasionFault != null && invasionFault.state != AquaFaultBase.FaultState.Active)
            available.Add(invasionFault);

        if (nutrientFault != null && nutrientFault.state != AquaFaultBase.FaultState.Active)
            available.Add(nutrientFault);

        if (temperatureFault != null && temperatureFault.state != AquaFaultBase.FaultState.Active)
            available.Add(temperatureFault);

        return available;
    }

    private AquaFaultBase PickRandomFault(List<AquaFaultBase> faults)
    {
        int randomIndex = Random.Range(0, faults.Count);
        return faults[randomIndex];
    }
}