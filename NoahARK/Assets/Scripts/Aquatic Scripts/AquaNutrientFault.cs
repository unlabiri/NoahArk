using UnityEngine;

public class AquaNutrientFault : AquaFaultBase
{
    [Header("References")]
    [SerializeField] private AquaticBiomeController biomeController;
    [SerializeField] private Renderer waterRenderer;

    [Header("Nutrient Drain")]
    [SerializeField] private float drainRate = 0.02f;
    [SerializeField] private float recoveryRate = 0.05f;

    [Header("Water Colors")]
    [SerializeField] private Color healthyFistColor = new Color(0.08f, 0.55f, 0.55f, 1f);
    [SerializeField] private Color healthySecondColor = new Color(0.05f, 0.35f, 0.45f, 1f);
    [SerializeField] private Color depletedFistColor = new Color(0.15f, 0.22f, 0.04f, 1f);
    [SerializeField] private Color depletedSecondColor = new Color(0.10f, 0.16f, 0.02f, 1f);

    [Header("Particles")]
    [SerializeField] private ParticleSystem nutrientParticles;
    [SerializeField] private ParticleSystem nutrientParticles1;
    [SerializeField] private float maxEmissionRate = 50f;
    [SerializeField] private float fadeOutLifetime = 13f;

    private Material waterMaterial;
    private AquaticBiomeState BiomeState => biomeController?.State;

    private void Start()
    {
        if (waterRenderer != null)
            waterMaterial = waterRenderer.material;

        ApplyWaterColor(1f);
        StopParticles(nutrientParticles);
        StopParticles(nutrientParticles1);
    }

    public override void Activate()
    {
        base.Activate();
        if (BiomeState != null)
            BiomeState.nutrientLevel = 1f;

        // Fresh start for new day
        ResetParticles(nutrientParticles);
        ResetParticles(nutrientParticles1);
    }

    private void Update()
    {
        if (BiomeState == null || waterMaterial == null) return;

        if (state == FaultState.Active)
        {
            BiomeState.nutrientLevel = Mathf.Clamp01(
                BiomeState.nutrientLevel - drainRate * Time.deltaTime);

            if (BiomeState.nutrientLevel <= 0f)
                state = FaultState.Failed;
        }
        else if (state == FaultState.Resolved)
        {
            BiomeState.nutrientLevel = Mathf.Clamp01(
                BiomeState.nutrientLevel + recoveryRate * Time.deltaTime);
        }

        ApplyWaterColor(BiomeState.nutrientLevel);
        UpdateParticles();
    }

    protected override void OnNewYear(int newYear)
    {
        if (state == FaultState.Active)
            drainRate = Mathf.Min(drainRate + 0.005f, 0.08f);
    }

    public void AddNutrients(float amount)
    {
        if (state != FaultState.Active) return;
        BiomeState.nutrientLevel = Mathf.Clamp01(BiomeState.nutrientLevel + amount);
        if (BiomeState.nutrientLevel >= 0.9f)
            state = FaultState.Resolved;
    }

    private void ApplyWaterColor(float nutrientLevel)
    {
        float t = 1f - nutrientLevel;
        waterMaterial.SetColor("FistColor",
            Color.Lerp(healthyFistColor, depletedFistColor, t));
        waterMaterial.SetColor("SecondColor",
            Color.Lerp(healthySecondColor, depletedSecondColor, t));
    }

    private void UpdateParticles()
    {
        if (state == FaultState.Active)
        {
            float rate = (1f - BiomeState.nutrientLevel) * maxEmissionRate;

            SetEmission(nutrientParticles, rate);
            SetEmission(nutrientParticles1, rate);

            PlayIfStopped(nutrientParticles);
            PlayIfStopped(nutrientParticles1);
        }
        else
        {
            StopParticles(nutrientParticles);
            StopParticles(nutrientParticles1);
        }
    }

    private void ResetParticles(ParticleSystem ps)
    {
        if (ps == null) return;
        var main = ps.main;
        main.startLifetime = 9999f;
        ps.Clear();
    }

    private void SetEmission(ParticleSystem ps, float rate)
    {
        if (ps == null) return;
        var emission = ps.emission;
        emission.rateOverTime = rate;
    }

    private void PlayIfStopped(ParticleSystem ps)
    {
        if (ps == null) return;
        var main = ps.main;
        main.startLifetime = 9999f;
        if (!ps.isPlaying) ps.Play();
    }

    private void StopParticles(ParticleSystem ps)
    {
        if (ps == null) return;

        // Stop spawning new ones
        var emission = ps.emission;
        emission.rateOverTime = 0f;

        // Force all existing particles to fade out over fadeOutLifetime
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.particleCount];
        int count = ps.GetParticles(particles);

        for (int i = 0; i < count; i++)
            particles[i].remainingLifetime = Mathf.Min(particles[i].remainingLifetime, fadeOutLifetime);

        ps.SetParticles(particles, count);
    }
}