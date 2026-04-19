using System.Collections.Generic;
using UnityEngine;

public class AquaticCoralEntity : MonoBehaviour
{
    public AquaticCoralState coralState;
    public AquaticBiomeController biomeController;

    [SerializeField] private float maxSafeTemperature = 82f;
    [SerializeField] private float minSafeTemperature = 65f;

    [Tooltip("Nutrient level below this is considered unsafe (0-1 scale)")]
    [SerializeField] private float minSafeNutrientLevel = 0.3f;

    // Damage intervals — start low, grow with each hit (same ladder as rainforest)
    [SerializeField] private float damageIntervalTemperature = 10f;
    [SerializeField] private float damageIntervalNutrient = 10f;

    private float timeInSafeConditions = 0f;
    private float timeToRegen = 20f;

    public void Initialize(AquaticBiomeController controller)
    {
        biomeController = controller;
    }

    private void Start()
    {
        coralState = new AquaticCoralState();

        if (biomeController == null)
            biomeController = FindObjectOfType<AquaticBiomeController>();

        biomeController?.RegisterCoral(this);
    }

    private void Update()
    {
        if (coralState.health == 0) return;
        if (!coralState.isAlive || biomeController == null) return;

        var biomeState = biomeController.State;

        bool tempUnsafe = biomeState.temperature > maxSafeTemperature
                       || biomeState.temperature < minSafeTemperature;

        bool nutrientUnsafe = biomeState.nutrientLevel < minSafeNutrientLevel;

        // Accumulate unsafe time
        if (tempUnsafe)
        {
            coralState.timeInUnsafeTemperature += Time.deltaTime;
            timeInSafeConditions = 0f;
            timeToRegen = 20f;
        }

        if (nutrientUnsafe)
        {
            coralState.timeAtLowNutrients += Time.deltaTime;
            timeInSafeConditions = 0f;
            timeToRegen = 20f;
        }

        // Temperature damage — intervals grow each hit so degradation is gradual
        if (tempUnsafe
            && coralState.timeInUnsafeTemperature >= damageIntervalTemperature
            && coralState.health > 0)
        {
            coralState.health -= 20;
            UpdateCoralStage();
            damageIntervalTemperature += 10f;
        }

        // Nutrient damage — same ladder pattern
        if (nutrientUnsafe
            && coralState.timeAtLowNutrients >= damageIntervalNutrient
            && coralState.health > 0)
        {
            coralState.health -= 20;
            UpdateCoralStage();
            damageIntervalNutrient += 10f;
        }

        // Recovery when both conditions are safe
        if (!tempUnsafe && !nutrientUnsafe)
        {
            coralState.timeInUnsafeTemperature = 0f;
            coralState.timeAtLowNutrients = 0f;

            timeInSafeConditions += Time.deltaTime;

            if (timeInSafeConditions >= timeToRegen)
            {
                if (coralState.health < 100)
                {
                    coralState.health += 10;
                    UpdateCoralStage();
                    coralState.health = Mathf.Min(coralState.health, 100);
                }
                timeToRegen += 20f;
            }
        }
    }

    public void UpdateCoralStage()
    {
        if (coralState.health <= 0)
        {
            coralState.health = 0;
            coralState.stage = CoralStage.Dead;
            coralState.isAlive = false;
        }
        else if (coralState.health <= 49)
        {
            coralState.stage = CoralStage.Bleached2;
        }
        else if (coralState.health <= 89)
        {
            coralState.stage = CoralStage.Bleached1;
        }
        else
        {
            coralState.stage = CoralStage.Healthy;
        }
    }
}