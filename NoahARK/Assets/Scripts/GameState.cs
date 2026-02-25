using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class GameState
{
    public int currentYear = 1;
    public int maxYears = 5;

    public WakePhase wakePhase = WakePhase.Sleeping;

    public float wakeSecondsRemaining = 0f;

    public float sleepSecondsRemaining = 0f;

    
}

public enum WakePhase
{
    Sleeping,
    Awake,
    Completed
}


