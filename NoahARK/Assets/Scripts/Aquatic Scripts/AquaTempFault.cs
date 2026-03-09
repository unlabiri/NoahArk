using UnityEngine;

public class AquaTempFault : AquaFaultBase
{
    [Header("Scenario Toggles")]
    public bool heatwaveActive = true;

    protected override void OnNewYear(int newYear)
    {
        if (!heatwaveActive)
        {
            Debug.Log($"{name}: No heatwave this year, no added stress.");
            return;
        }

        // Heat stress accumulates each year
        stress += stressPerYear;
    }

    public void ApplyCooling(int amount)
    {
        if (state != FaultState.Active) return;

        stress = Mathf.Max(0, stress - amount);
        RecomputeSeverity();

        Debug.Log($"{name}: Cooling applied (-{amount}). Now stress={stress}, severity={severity}");

        if (stress == 0)
        {
            state = FaultState.Resolved;
            Debug.Log($"{name}: RESOLVED ");
        }
    }
}