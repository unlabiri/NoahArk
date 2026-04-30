using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class AquaticBiomeState
{
    public BiomeHealthState health = BiomeHealthState.Healthy;

    public float temperature = 75f;
    public float nutrientLevel = 1f; // 0.0 depleted -> 1.0 fully nourished

    public bool invasiveSpeciesPresent = false;

    public int totalCoralCount = 0;
    public int totalAliveCoralCount = 0;
    public int totalDeadCoralCount = 0;
}