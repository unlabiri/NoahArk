using UnityEngine;

public class AquaNutrientFault : AquaFaultBase
{
    [Header("Visuals")]
    [SerializeField] private Renderer planeRenderer;

    [SerializeField] private Material normalWaterMaterial;
    [SerializeField] private Material lowMaterial;
    [SerializeField] private Material mediumMaterial;
    [SerializeField] private Material highMaterial;
    [SerializeField] private Material criticalMaterial;
    [SerializeField] private Material failedMaterial;

    private void Start()
    {
        UpdatePlaneMaterial();
    }

    public override void Activate()
    {
        base.Activate();
        UpdatePlaneMaterial();
    }

    public override void Deactivate()
    {
        base.Deactivate();
        UpdatePlaneMaterial();
    }

    protected override void OnNewYear(int newYear)
    {
        stress += stressPerYear;
        RecomputeSeverity();
        UpdatePlaneMaterial();
    }

    private void Update()
    {
        if (planeRenderer != null)
        {
            UpdatePlaneMaterial();
        }
    }

    private void UpdatePlaneMaterial()
    {
        if (planeRenderer == null) return;

        switch (state)
        {
            case FaultState.Inactive:
            case FaultState.Resolved:
                if (normalWaterMaterial != null)
                    planeRenderer.material = normalWaterMaterial;
                break;

            case FaultState.Failed:
                if (failedMaterial != null)
                    planeRenderer.material = failedMaterial;
                break;

            case FaultState.Active:
                switch (severity)
                {
                    case Severity.Low:
                        if (lowMaterial != null)
                            planeRenderer.material = lowMaterial;
                        break;

                    case Severity.Medium:
                        if (mediumMaterial != null)
                            planeRenderer.material = mediumMaterial;
                        break;

                    case Severity.High:
                        if (highMaterial != null)
                            planeRenderer.material = highMaterial;
                        break;

                    case Severity.Critical:
                        if (criticalMaterial != null)
                            planeRenderer.material = criticalMaterial;
                        break;
                }
                break;
        }
    }
}