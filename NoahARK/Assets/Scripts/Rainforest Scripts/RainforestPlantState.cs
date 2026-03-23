using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]

public class RainforestPlantState
{
    public PlantStage stage = PlantStage.Healthy;
    public bool isAlive = true;
    public float timeInUnsafeTemperature = 0f;
    public float timeInUnsafeHumidity = 0f;
    public float timeInfected = 0f;
    public float health = 100f;

}

public enum PlantStage
{
    Healthy,
    Wilted1,
    Wilted2,
    Dead
}
