using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainforestBiomeController : MonoBehaviour
{
    // Start is called before the first frame update
    public RainforestBiomeState State { get; private set; } = new RainforestBiomeState();
    [SerializeField] private List<RainforestPlantEntity> plants = new();
    private void Start()
    {
        foreach (var plant in plants)
        {
            if (plant != null)
            {
                plant.Initialize(this);
            }
        }

        RefreshPlantCounts();
        UpdateBiomeHealthState();
    }

    // Update is called once per frame
    private void Update()
    {
        AdvanceTime();
        ProcessScheduledEvents();
    }

    public void RegisterPlant(PlantEntity plant)
    {
        if (plant == null || plants.Contains(plant))
            return;

        plants.Add(plant);
        RefreshPlantCounts();
        UpdateBiomeHealthState();
    }

    private void RefreshPlantCounts()
    {
        State.totalPlantCount = plants.Count;
        State.totalAlivePlantCount = 0;
        State.totalDeadPlantCount = 0;

        foreach (var plant in plants)
        {
            if (plant == null) continue;

            if (plant.State.isAlive)
                State.totalAlivePlantCount++;
            else
                State.totalDeadPlantCount++;
        }
    }

    private void UpdateBiomeHealthState()
    {
        if (State.totalPlantCount <= 0)
        {
            State.biomeHealthState = BiomeHealthState.Extinct;
            return;
        }

        float alivePercent = (float)State.totalAlivePlantCount / State.totalPlantCount;

        if (alivePercent >= 0.75f)
            State.biomeHealthState = BiomeHealthState.Healthy;
        else if (alivePercent >= 0.50f)
            State.biomeHealthState = BiomeHealthState.Vulnerable;
        else if (alivePercent > 0f)
            State.biomeHealthState = BiomeHealthState.Endangered;
        else
            State.biomeHealthState = BiomeHealthState.Extinct;
    }
}
