using UnityEngine;

/// <summary>
/// Mirrors RainforestPlantVisualController.
/// Automatically tracks coralState.stage each frame and swaps materials on change.
/// Also exposes public methods so AquaTemperatureFault can force visual states directly
/// by severity without needing to go through the entity health system.
/// </summary>
public class CoralBleachingVisual : MonoBehaviour
{
    [SerializeField] private AquaticCoralEntity coral;

    [SerializeField] private GameObject liveModel;
    [SerializeField] private GameObject deadModel;

    [SerializeField] private Renderer coralRenderer;

    [SerializeField] private Material healthyMaterial;
    [SerializeField] private Material bleached1Material;
    [SerializeField] private Material bleached2Material;
    [SerializeField] private Material deadMaterial;

    private CoralStage lastAppliedStage;

    void Start()
    {
        if (coral == null)
            coral = GetComponent<AquaticCoralEntity>();

        ApplyVisualState();
    }

    void Update()
    {
        if (coral == null) return;

        if (coral.coralState.stage != lastAppliedStage)
            ApplyVisualState();
    }

    private void ApplyVisualState()
    {
        if (coral == null) return;
        ForceStage(coral.coralState.stage);
    }

    // -------------------------------------------------------------------------
    // Public API for AquaTemperatureFault — maps severity levels to stages
    // -------------------------------------------------------------------------

    public void ApplyHealthy() => ForceStage(CoralStage.Healthy);
    public void ApplyLowBleaching() => ForceStage(CoralStage.Bleached1);
    public void ApplyMediumBleaching() => ForceStage(CoralStage.Bleached1);
    public void ApplyHighBleaching() => ForceStage(CoralStage.Bleached2);
    public void ApplyFullBleaching() => ForceStage(CoralStage.Dead);

    // -------------------------------------------------------------------------

    private void ForceStage(CoralStage stage)
    {
        lastAppliedStage = stage;

        switch (stage)
        {
            case CoralStage.Healthy:
                SetActiveModel(useLive: true);
                coralRenderer.material = healthyMaterial;
                break;

            case CoralStage.Bleached1:
                SetActiveModel(useLive: true);
                coralRenderer.material = bleached1Material;
                break;

            case CoralStage.Bleached2:
                SetActiveModel(useLive: true);
                coralRenderer.material = bleached2Material;
                break;

            case CoralStage.Dead:
                SetActiveModel(useLive: false);
                if (deadModel != null)
                    deadModel.GetComponent<Renderer>()?.SetMaterials(new() { deadMaterial });
                else
                    coralRenderer.material = deadMaterial;
                break;
        }
    }

    private void SetActiveModel(bool useLive)
    {
        if (liveModel != null) liveModel.SetActive(useLive);
        if (deadModel != null) deadModel.SetActive(!useLive);
    }
}