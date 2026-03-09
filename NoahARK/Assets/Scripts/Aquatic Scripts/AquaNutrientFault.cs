using UnityEngine;

public class AquaNutrientFault : AquaFaultBase
{
    [Header("Scenario Toggles")]
    public bool runoffSourceStillActive = true;

    protected override void OnNewYear(int newYear)
    {
        if (runoffSourceStillActive)
        {
            stress += stressPerYear;
            Debug.Log($"{name}: RUNOFF active.");
        }
        else
        {
            //  natural recovery when source is removed
            stress = Mathf.Max(0, stress - 1);
            Debug.Log($"{name}: RUNOFF FIXED.");
        }
    }

    public void ReduceNutrients(int amount)
    {
        if (state != FaultState.Active) return;

        stress = Mathf.Max(0, stress - amount);
        RecomputeSeverity();

        Debug.Log($"{name}: stress={stress}, severity={severity}");

        if (stress == 0)
        {
            state = FaultState.Resolved;
            Debug.Log($"{name}: RESOLVED");
        }
    }

    public void FixRunoffSource()
    {
        runoffSourceStillActive = false;
        Debug.Log($"{name}: RUNOFF FIXED.");
    }
}