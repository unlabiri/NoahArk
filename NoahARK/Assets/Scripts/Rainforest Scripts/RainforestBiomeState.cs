using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class RainforestBiomeState
{
    public BiomeHealthState health = BiomeHealthState.Health;
    public float temperature = 80f;
    public float humidity = .99f;
    public bool invasiveSpecies = false;
    public int totalPlantCount = 0;
    public int totalAlivePlantCount = 0;
    public int totalDeadPlantCount = 0;

}

public enum BiomeHealthState
{
    Health, // 100% - 71%
    Vulnerable, // 70% - 31%
    Endangered, // 30% - 1%
    Extinct // 0%
}