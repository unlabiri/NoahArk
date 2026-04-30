using UnityEngine;

public class AquaNutrientFault : AquaFaultBase
{
    [Header("References")]
    [SerializeField] private AquaticBiomeController biomeController;
    [SerializeField] private Renderer waterRenderer;   // Drag Watersurface_(2) here

    [Header("Nutrient Drain")]
    [Tooltip("How much nutrientLevel drops per second while fault is active")]
    [SerializeField] private float drainRate = 0.02f;

    [Tooltip("How much nutrientLevel recovers per second once resolved")]
    [SerializeField] private float recoveryRate = 0.05f;

    [Header("Water Colors")]
    [Tooltip("The normal healthy water color — match your current FistColor in the shader")]
    [SerializeField] private Color healthyFistColor = new Color(0.08f, 0.55f, 0.55f, 1f);
    [SerializeField] private Color healthySecondColor = new Color(0.05f, 0.35f, 0.45f, 1f);

    [Tooltip("The gross vomit green at full nutrient depletion")]
    [SerializeField] private Color depletedFistColor = new Color(0.15f, 0.22f, 0.04f, 1f);
    [SerializeField] private Color depletedSecondColor = new Color(0.10f, 0.16f, 0.02f, 1f);

    private Material waterMaterial;
    private AquaticBiomeState BiomeState => biomeController?.State;

    private void Start()
    {
        if (waterRenderer != null)
            waterMaterial = waterRenderer.material;

        // Make sure water starts healthy
        ApplyWaterColor(1f);
    }

    public override void Activate()
    {
        base.Activate();
        if (BiomeState != null)
            BiomeState.nutrientLevel = 1f;
    }

    private void Update()
    {
        if (BiomeState == null || waterMaterial == null) return;

        if (state == FaultState.Active)
        {
            // Drain nutrient level over time
            BiomeState.nutrientLevel = Mathf.Clamp01(
                BiomeState.nutrientLevel - drainRate * Time.deltaTime);

            // Check if fully depleted — escalate to failed
            if (BiomeState.nutrientLevel <= 0f)
                state = FaultState.Failed;
        }
        else if (state == FaultState.Resolved)
        {
            // Slowly recover water color back to healthy
            BiomeState.nutrientLevel = Mathf.Clamp01(
                BiomeState.nutrientLevel + recoveryRate * Time.deltaTime);
        }

        // Always drive water color from current nutrient level
        ApplyWaterColor(BiomeState.nutrientLevel);
    }

    protected override void OnNewYear(int newYear)
    {
        // Nutrient faults get worse each year if unresolved
        if (state == FaultState.Active)
            drainRate = Mathf.Min(drainRate + 0.005f, 0.08f);
    }

    /// <summary>
    /// Called by AquaNutrientRemedyStation when the player doses nutrients.
    /// </summary>
    public void AddNutrients(float amount)
    {
        if (state != FaultState.Active) return;

        BiomeState.nutrientLevel = Mathf.Clamp01(BiomeState.nutrientLevel + amount);

        if (BiomeState.nutrientLevel >= 0.9f)
            state = FaultState.Resolved;
    }

    private void ApplyWaterColor(float nutrientLevel)
    {
        // nutrientLevel 1 = healthy colors, 0 = gross green
        float t = 1f - nutrientLevel;

        waterMaterial.SetColor("FistColor",
            Color.Lerp(healthyFistColor, depletedFistColor, t));
        waterMaterial.SetColor("SecondColor",
            Color.Lerp(healthySecondColor, depletedSecondColor, t));
    }
}