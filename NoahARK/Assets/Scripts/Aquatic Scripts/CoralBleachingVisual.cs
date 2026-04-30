using UnityEngine;

public class CoralBleachingVisual : MonoBehaviour
{
    [SerializeField] private AquaticCoralEntity coral;
    [SerializeField] private Renderer coralRenderer;

    private AquaticBiomeController biomeController;
    private CoralStage lastAppliedStage;

    void Start()
    {
        if (coral == null)
            coral = GetComponent<AquaticCoralEntity>();

        biomeController = FindObjectOfType<AquaticBiomeController>();

        ApplyVisualState();
    }

    void Update()
    {
        if (coral == null) return;

        if (coral.coralState.stage != lastAppliedStage)
            ApplyVisualState();
    }

    private void ApplyVisualState() => ForceStage(coral.coralState.stage);

    public void ApplyHealthy() => ForceStage(CoralStage.Healthy);
    public void ApplyLowBleaching() => ForceStage(CoralStage.Bleached1);
    public void ApplyMediumBleaching() => ForceStage(CoralStage.Bleached1);
    public void ApplyHighBleaching() => ForceStage(CoralStage.Bleached2);
    public void ApplyFullBleaching() => ForceStage(CoralStage.Dead);

    private void ForceStage(CoralStage stage)
    {
        if (coralRenderer == null || biomeController == null) return;

        lastAppliedStage = stage;

        switch (stage)
        {
            case CoralStage.Healthy: coralRenderer.material = biomeController.healthyMaterial; break;
            case CoralStage.Bleached1: coralRenderer.material = biomeController.bleached1Material; break;
            case CoralStage.Bleached2: coralRenderer.material = biomeController.bleached2Material; break;
            case CoralStage.Dead: coralRenderer.material = biomeController.deadMaterial; break;
        }
    }
}