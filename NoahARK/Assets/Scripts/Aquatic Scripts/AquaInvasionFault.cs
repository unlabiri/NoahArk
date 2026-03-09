using UnityEngine;

public class AquaInvasionFault : AquaFaultBase
{
    [Header("Control Rules")]
    public int controlThreshold = 1;       
    public int yearsToMaintainControl = 2; 

    private int controlStreak = 0;

    protected override void OnNewYear(int newYear)
    {
        stress += stressPerYear;

        if (stress <= controlThreshold) controlStreak++;
        else controlStreak = 0;

        Debug.Log($"{name}: Invasion spread. controlStreak={controlStreak}/{yearsToMaintainControl}");

        if (controlStreak >= yearsToMaintainControl)
        {
            state = FaultState.Resolved;
            Debug.Log($"{name}: Resolved");
        }
    }

    public void RemoveInvasive(int amount)
    {
        if (state != FaultState.Active) return;

        stress = Mathf.Max(0, stress - amount);
        RecomputeSeverity();

        Debug.Log($"{name}: Resolved");
    }
}