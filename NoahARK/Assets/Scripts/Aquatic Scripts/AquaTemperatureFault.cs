using UnityEngine;

public class AquaTemperatureFault : AquaFaultBase
{
    [Header("Temperature Control")]
    public int safeThreshold = 1;
    public int yearsToRecover = 2;

    [Header("Bleaching Visual")]
    [SerializeField] private CoralBleachingVisual coralVisual;

    [Header("Optional Alarm Light")]
    [SerializeField] private Renderer alarmRenderer;
    [SerializeField] private Light alarmLight;
    [SerializeField] private string glowingMaterialName = "lambert2";

    [SerializeField] private Color inactiveEmission = Color.green;
    [SerializeField] private Color activeEmission = new Color(1f, 0.4f, 0f);
    [SerializeField] private Color resolvedEmission = Color.green;
    [SerializeField] private Color failedEmission = Color.black;

    private Material targetMaterial;
    private int recoveryStreak = 0;

    private void Start()
    {
        if (alarmRenderer != null)
        {
            Material[] mats = alarmRenderer.materials;

            for (int i = 0; i < mats.Length; i++)
            {
                if (mats[i] != null && mats[i].name.Contains(glowingMaterialName))
                {
                    targetMaterial = mats[i];
                    targetMaterial.EnableKeyword("_EMISSION");
                    break;
                }
            }

            if (targetMaterial == null && mats.Length > 0)
            {
                Debug.LogWarning($"{name}: Could not find material '{glowingMaterialName}', defaulting to first material.");
                targetMaterial = mats[0];
                targetMaterial.EnableKeyword("_EMISSION");
            }
        }

        UpdateTemperatureVisual();
    }

    public override void Activate()
    {
        base.Activate();
        recoveryStreak = 0;
        UpdateTemperatureVisual();
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
        UpdateTemperatureVisual();
    }

    public void CoolWater(int amount)
    {
        if (state != FaultState.Active) return;

        stress = Mathf.Max(0, stress - amount);
        RecomputeSeverity();

        if (stress <= 0)
            state = FaultState.Resolved;

        UpdateTemperatureVisual();
    }

    private void UpdateTemperatureVisual()
    {
        UpdateBleachingVisual();
        UpdateAlarmVisual();
    }

    private void UpdateBleachingVisual()
    {
        if (coralVisual == null) return;

        switch (state)
        {
            case FaultState.Inactive:
                coralVisual.ApplyHealthy();
                break;

            case FaultState.Active:
                switch (severity)
                {
                    case Severity.Low:
                        coralVisual.ApplyLowBleaching();
                        break;
                    case Severity.Medium:
                        coralVisual.ApplyMediumBleaching();
                        break;
                    case Severity.High:
                        coralVisual.ApplyHighBleaching();
                        break;
                    case Severity.Critical:
                        coralVisual.ApplyFullBleaching();
                        break;
                    default:
                        coralVisual.ApplyLowBleaching();
                        break;
                }
                break;

            case FaultState.Resolved:
                coralVisual.ApplyHealthy();
                break;

            case FaultState.Failed:
                coralVisual.ApplyFullBleaching();
                break;
        }
    }

    private void UpdateAlarmVisual()
    {
        Color currentColor = inactiveEmission;

        switch (state)
        {
            case FaultState.Inactive:
                currentColor = inactiveEmission;
                break;
            case FaultState.Active:
                currentColor = activeEmission;
                break;
            case FaultState.Resolved:
                currentColor = resolvedEmission;
                break;
            case FaultState.Failed:
                currentColor = failedEmission;
                break;
        }

        if (targetMaterial != null)
        {
            targetMaterial.EnableKeyword("_EMISSION");
            targetMaterial.SetColor("_EmissionColor", currentColor * 2f);
        }

        if (alarmLight != null)
        {
            alarmLight.color = currentColor;
            alarmLight.enabled = currentColor != Color.black;
        }
    }
}