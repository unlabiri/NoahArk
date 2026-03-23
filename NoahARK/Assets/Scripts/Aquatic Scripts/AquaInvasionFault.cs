using UnityEngine;

public class AquaInvasionFault : AquaFaultBase
{
    [Header("Control Rules")]
    public int controlThreshold = 1;
    public int yearsToMaintainControl = 2;

    [Header("Visuals")]
    [SerializeField] private Renderer sphereRenderer;
    [SerializeField] private Color inactiveColor = Color.gray;
    [SerializeField] private Color activeColor = Color.red;
    [SerializeField] private Color resolvedColor = Color.green;
    [SerializeField] private Color failedColor = Color.black;

    private int controlStreak = 0;

    private void Start()
    {
        UpdateSphereColor();
    }

    public override void Activate()
    {
        base.Activate();
        controlStreak = 0;
        UpdateSphereColor();
        Debug.Log($"{name}: ubvasuib actuvate cakked");

    }

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

        UpdateSphereColor();
    }

    public void RemoveInvasive(int amount)
    {
        if (state != FaultState.Active) return;

        stress = Mathf.Max(0, stress - amount);
        RecomputeSeverity();
        UpdateSphereColor();
    }

    private void UpdateSphereColor()
    {
        if (sphereRenderer == null) return;

        switch (state)
        {
            case FaultState.Inactive:
                sphereRenderer.material.color = inactiveColor;
                break;
            case FaultState.Active:
                sphereRenderer.material.color = activeColor;
                break;
            case FaultState.Resolved:
                sphereRenderer.material.color = resolvedColor;
                break;
            case FaultState.Failed:
                sphereRenderer.material.color = failedColor;
                break;
        }
    }
}