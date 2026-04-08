using UnityEngine;

public class AquaInvasionFault : AquaFaultBase
{
    [Header("Control Rules")]
    public int controlThreshold = 1;
    public int yearsToMaintainControl = 2;

    [Header("Alarm Light Visuals")]
    [SerializeField] private Renderer alarmRenderer;
    [SerializeField] private Light alarmLight;

    [SerializeField] private string glowingMaterialName = "lambert2";

    [SerializeField] private Color inactiveEmission = Color.green;
    [SerializeField] private Color activeEmission = Color.red;
    [SerializeField] private Color resolvedEmission = Color.green;
    [SerializeField] private Color failedEmission = Color.black;

    private Material targetMaterial;
    private int controlStreak = 0;

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

        UpdateAlarmVisual();
    }

    public override void Activate()
    {
        base.Activate();
        controlStreak = 0;
        UpdateAlarmVisual();
        Debug.Log($"{name}: Invasive fault activated.");
    }

    protected override void OnNewYear(int newYear)
    {
        if (state != FaultState.Active)
            return;

        stress += stressPerYear;

        if (stress <= controlThreshold) controlStreak++;
        else controlStreak = 0;

        if (controlStreak >= yearsToMaintainControl)
        {
            state = FaultState.Resolved;
            Debug.Log($"{name}: Invasive fault resolved.");
        }

        if (stress >= failAtStress)
        {
            state = FaultState.Failed;
            Debug.Log($"{name}: Invasive fault failed.");
        }

        UpdateAlarmVisual();
    }

    public void RemoveInvasive(int amount)
    {
        if (state != FaultState.Active)
            return;

        stress = Mathf.Max(0, stress - amount);
        RecomputeSeverity();

        if (stress <= 0)
            state = FaultState.Resolved;

        UpdateAlarmVisual();
    }

    public void ResolveInvasiveFault()
    {
        state = FaultState.Inactive;
        stress = 0;
        controlStreak = 0;

        Debug.Log($"{name}: Invasive fault cleared.");

        UpdateAlarmVisual();
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