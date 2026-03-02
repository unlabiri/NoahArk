using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WakeCycleManager : MonoBehaviour
{
    [SerializeField] private float wakeDurationSeconds = 30f;
    [SerializeField] private float sleepDurationSeconds = 10f;
    public GameState State { get; private set; }

    public event Action<WakePhase> OnStateChange;
    public event Action<int> OnYearChange;
    public event Action<float> OnSecondsRemaining;

    private void Awake()
    {
        State = new GameState();


    }

    private void Start()
    {
        StartWakeCycle();
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        // If you have completed the wake phase return the game is over
        if (State.wakePhase == WakePhase.Completed) return;
        Console.WriteLine(State.currentYear);
        // if the user is in the sleeping phase the timer will countdown  10 seconds until the new awake cycle.
        if (State.wakePhase == WakePhase.Sleeping)
        {
            State.sleepSecondsRemaining -= Time.deltaTime;

            if(State.sleepSecondsRemaining < 0)
            {
                StartWakeCycle();
                
                // when the new awake cycle is started this will be considered a new year
                State.currentYear += 1;
                OnYearChange?.Invoke(State.currentYear);
                OnStateChange?.Invoke(State.wakePhase);

            }
        }
        // if you are in the awake phase begin counting down 30 seconds until the new sleep cycle begins
        if (State.wakePhase == WakePhase.Awake)
        {
            State.wakeSecondsRemaining -= Time.deltaTime;
            if (State.wakeSecondsRemaining < 0)
            {
                State.wakeSecondsRemaining = 0f;
                StartSleepCycle();
                OnStateChange?.Invoke(State.wakePhase);
            }
        }
        

        
        // after max years the game is completed and the wake cycle no longer needs to go on
        if (State.currentYear > State.maxYears)
        {
            State.wakePhase = WakePhase.Completed;
            OnStateChange?.Invoke(State.wakePhase);
        }

    }

    void StartWakeCycle()
    {
        State.wakePhase = WakePhase.Awake;
        State.wakeSecondsRemaining = wakeDurationSeconds;
    }

    void StartSleepCycle()
    {

        State.wakePhase = WakePhase.Sleeping;
        State.sleepSecondsRemaining = sleepDurationSeconds;

    }
}


