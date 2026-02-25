using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WakeCycleManager : MonoBehaviour
{
    [SerializeField] private float wakeDurationSeconds = 30f;
    [SerializeField] private float sleepDurationSeconds = 10f;
    public GameState State { get; private set; }

    public event Action<GameState> OnStateChange;
    public event Action<int> OnYearChange;
    public event Action<float> OnWakeTick;

    private bool _isInitialized = false;
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
        if (State.wakePhase == WakePhase.Completed) return;
        Console.WriteLine(State.currentYear);
        if (State.wakePhase == WakePhase.Sleeping)
        {
            State.sleepSecondsRemaining -= Time.deltaTime;

            if(State.sleepSecondsRemaining <= 0)
            {
                StartWakeCycle();
                State.currentYear += 1;

            }
        }
        if (State.wakePhase == WakePhase.Awake)
        {
            State.wakeSecondsRemaining -= Time.deltaTime;
        }
        

        if (State.wakeSecondsRemaining <= 0)
        {
            State.wakeSecondsRemaining = 0f;
            StartSleepCycle();
        }

        if (State.currentYear > State.maxYears)
        {
            State.wakePhase = WakePhase.Completed;
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


