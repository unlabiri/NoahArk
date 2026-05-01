using UnityEngine;

public class AquaTemperatureFault : AquaFaultBase
{
    [Header("Temperature Control")]
    public int safeThreshold = 1;
    public int yearsToRecover = 2;

    private int recoveryStreak = 0;

    public override void Activate()
    {
        base.Activate();
        recoveryStreak = 0;
    }

    protected override void OnNewYear(int newYear)
    {
        stress += stressPerYear;

        if (stress <= safeThreshold)
            recoveryStreak++;
        else
            recoveryStreak = 0;

        if (recoveryStreak >= yearsToRecover)
            state = FaultState.Resolved;

        RecomputeSeverity();
    }

    public void CoolWater(int amount)
    {
        if (state != FaultState.Active) return;

        stress = Mathf.Max(0, stress - amount);
        RecomputeSeverity();

        if (stress <= 0)
            state = FaultState.Resolved;

        Debug.Log($"[AquaTemperatureFault] Cooled by {amount}, stress now {stress}");
    }
    public void ResolveWithCrystal()
    {
        if (state != FaultState.Active) return;
        stress = 0;
        recoveryStreak = 0;
        state = FaultState.Resolved;
        Debug.Log("[AquaTemperatureFault] Crystal used — temperature fault resolved.");
    }
}