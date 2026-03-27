using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class RainforestBiomeState
{
    public BiomeHealthState health = BiomeHealthState.Healthy;
    public float temperature = 70f;
    public float humidity = .8f;
    public int totalPlantCount = 0;
    public int totalAlivePlantCount = 0;
    public int totalDeadPlantCount = 0;

}

public enum BiomeHealthState
{
    Healthy, // 100% - 71%
    Vulnerable, // 70% - 31%
    Endangered, // 30% - 1%
    Extinct // 0%
}