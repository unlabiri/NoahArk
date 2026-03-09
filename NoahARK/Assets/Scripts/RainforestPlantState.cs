using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]

public class RainforestPlantState
{
    public PlantStage stage = PlantStage.Healthy;
    public float timeInUnsafeTemperature = 0f;
    public float health = 100f;

}

public enum PlantStage
{
    Healthy,
    Wilted1,
    Wilted2,
    Dead
}
