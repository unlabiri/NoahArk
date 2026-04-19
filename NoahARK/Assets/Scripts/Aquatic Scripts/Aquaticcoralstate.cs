using System;
using UnityEngine;

[Serializable]
public class AquaticCoralState
{
    public CoralStage stage = CoralStage.Healthy;
    public bool isAlive = true;
    public float timeInUnsafeTemperature = 0f;
    public float timeAtLowNutrients = 0f;
    public float health = 100f;
}

public enum CoralStage
{
    Healthy,    // 90 - 100
    Bleached1,  // 50 - 89  (early bleaching, color fading)
    Bleached2,  // 1  - 49  (heavy bleaching, mostly white)
    Dead        // 0        (fully white/grey, no recovery)
}