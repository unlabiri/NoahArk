using UnityEngine;

public class AquaTemperatureFault : AquaFaultBase
{
    [Header("Temperature Control")]
    public int safeThreshold = 1;
    public int yearsToRecover = 2;

    [Header("References")]
    [SerializeField] private AquaticBiomeController biomeController;
    [SerializeField] private float normalTemperature = 75f;
    [SerializeField] private float maxTemperature = 95f;
    [SerializeField] private float heatRiseSpeed = 2f;

    private AquaticBiomeState BiomeState => biomeController?.State;
    private int recoveryStreak = 0;

    public override void Activate()
    {
        base.Activate();
        recoveryStreak = 0;
    }

    private void Update()
    {
        if (BiomeState == null) return;

        if (state == FaultState.Active)
        {
            BiomeState.temperature = Mathf.Min(
                BiomeState.temperature + heatRiseSpeed * Time.deltaTime,
                maxTemperature);
        }
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

        if (BiomeState != null)
            BiomeState.temperature = normalTemperature;

        Debug.Log("[AquaTemperatureFault] Crystal used — temperature fault resolved.");
    }
}