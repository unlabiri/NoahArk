using UnityEngine;

public struct ArcticStateData
{
    public string health;
}

public class ArcticBiomeController : MonoBehaviour
{
    public enum ArcticHealth { Healthy, Damaged, Critical, Extinct }

    [Header("Status")]
    public ArcticHealth currentHealth = ArcticHealth.Healthy;

    [Header("Weather Effects")]
    public GameObject snowEffect; // NEW: Drag your Particle System here!

    [Header("Igloo Models")]
    public GameObject iglooHealthy;
    public GameObject iglooDamaged;
    public GameObject iglooSeverelyDamaged;

    [Header("Icicle Models")]
    public GameObject iciclesHealthy;
    public GameObject iciclesDamaged;
    public GameObject iciclesSeverelyDamaged;

    public ArcticStateData State => new ArcticStateData { health = currentHealth.ToString() };

    private void Start()
    {
        UpdateVisuals();
    }

    public void UpdateTemperature(float currentTemp)
    {
        if (currentHealth == ArcticHealth.Extinct) return;

        ArcticHealth newState = currentHealth;

        if (currentTemp >= 70f) newState = ArcticHealth.Extinct;
        else if (currentTemp >= 52f) newState = ArcticHealth.Critical;
        else if (currentTemp >= 32f) newState = ArcticHealth.Damaged;

        if ((int)newState > (int)currentHealth)
        {
            currentHealth = newState;
            UpdateVisuals();
            Debug.Log($"<color=cyan>[Arctic]</color> Permanent damage reached: {currentHealth}");
        }
    }

    private void UpdateVisuals()
    {
        // 1. Handle Weather (SNOW)
        if (snowEffect != null)
        {
            // Only snow if the biome is perfectly healthy
            snowEffect.SetActive(currentHealth == ArcticHealth.Healthy);
        }

        // 2. Handle Igloo Logic
        if (iglooHealthy) iglooHealthy.SetActive(false);
        if (iglooDamaged) iglooDamaged.SetActive(false);
        if (iglooSeverelyDamaged) iglooSeverelyDamaged.SetActive(false);

        if (currentHealth == ArcticHealth.Healthy)
        {
            if (iglooHealthy) iglooHealthy.SetActive(true);
        }
        else if (currentHealth == ArcticHealth.Damaged)
        {
            if (iglooDamaged) iglooDamaged.SetActive(true);
        }
        else
        {
            if (iglooSeverelyDamaged) iglooSeverelyDamaged.SetActive(true);
        }

        // 3. Handle Icicle Logic
        if (iciclesHealthy) iciclesHealthy.SetActive(false);
        if (iciclesDamaged) iciclesDamaged.SetActive(false);
        if (iciclesSeverelyDamaged) iciclesSeverelyDamaged.SetActive(false);

        if (currentHealth == ArcticHealth.Healthy)
        {
            if (iciclesHealthy) iciclesHealthy.SetActive(true);
        }
        else if (currentHealth == ArcticHealth.Damaged)
        {
            if (iciclesDamaged) iciclesDamaged.SetActive(true);
        }
        else
        {
            if (iciclesSeverelyDamaged) iciclesSeverelyDamaged.SetActive(true);
        }
    }
}