using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainforestBiomeController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private RainforestBiomeState state = new RainforestBiomeState();
    public RainforestBiomeState State => state;
    [SerializeField] public List<RainforestPlantEntity> plants = new();
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
    public void Update()
    {

        RefreshPlantCounts();
        UpdateBiomeHealthState();
    }



    public void RegisterPlant(RainforestPlantEntity plant)
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
            if (plant.plantState.isAlive)
                State.totalAlivePlantCount++;

            else
                State.totalDeadPlantCount++;
        }
    }

    private void UpdateBiomeHealthState()
    {
        if (State.totalPlantCount <= 0)
        {
            State.health = BiomeHealthState.Extinct;
            return;
        }

        float alivePercent = (float)State.totalAlivePlantCount / State.totalPlantCount;
        

        if (alivePercent >= 0.75f)
            State.health = BiomeHealthState.Healthy;
        else if (alivePercent >= 0.50f)
            State.health = BiomeHealthState.Vulnerable;
        else if (alivePercent > 0f)
            State.health = BiomeHealthState.Endangered;
        else
            State.health = BiomeHealthState.Extinct;
    }
}
