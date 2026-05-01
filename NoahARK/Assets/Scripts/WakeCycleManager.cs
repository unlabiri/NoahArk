using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Defines a point in time (year + seconds remaining in the wake phase)
/// at which OnScheduledEvent fires. Add entries in the Inspector.
/// Note: secondsRemaining counts DOWN from wakeDurationSeconds.
/// e.g. year=1, secondsRemaining=20 fires when 20s are left in year 1's wake phase.
/// </summary>
[Serializable]
public class WakeCycleScheduledEvent
{
    public int year = 1;
    public float secondsRemaining = 20f;
    public string targetBiome = "";
    [HideInInspector] public bool hasTriggered = false;
}

public class WakeCycleManager : MonoBehaviour
{
    [SerializeField] private float wakeDurationSeconds = 480f;
    [SerializeField] private float sleepDurationSeconds = 20f;
    public GameState State { get; private set; }

    public event Action<WakePhase> OnStateChange;
    public event Action<int> OnYearChange;
    public event Action<bool> OnComplete;

    private bool _warningPlayed = false;
    public AudioSource sleepWarning;
    [Header("Scheduled Events")]
    [SerializeField] private List<WakeCycleScheduledEvent> scheduledEvents = new();

    /// <summary>
    /// Fires when a scheduled event's year + secondsRemaining threshold is crossed.
    /// Subscribers (e.g. FaultManager) hook into this to know when to act.
    /// </summary>
    public event Action<WakeCycleScheduledEvent> OnScheduledEvent;

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
        if (State.wakePhase == WakePhase.Completed)
        {
            OnComplete?.Invoke(true);
            return;
        } 
            
        Console.WriteLine(State.currentYear);
        // if the user is in the sleeping phase the timer will countdown  10 seconds until the new awake cycle.
        if (State.wakePhase == WakePhase.Sleeping)
        {
            State.sleepSecondsRemaining -= Time.deltaTime;
            _warningPlayed = false;

            if (State.sleepSecondsRemaining < 0)
            {
                State.currentYear += 1;
                StartWakeCycle();

                // when the new awake cycle is started this will be considered a new year
                
                OnYearChange?.Invoke(State.currentYear);
                OnStateChange?.Invoke(State.wakePhase);

            }
        }
        // if you are in the awake phase begin counting down 30 seconds until the new sleep cycle begins
        if (State.wakePhase == WakePhase.Awake)
        {
            State.wakeSecondsRemaining -= Time.deltaTime;

            // Check all scheduled events — fire any whose threshold we've just crossed
            foreach (var e in scheduledEvents)
            {
                if (!e.hasTriggered
                    && e.year == State.currentYear
                    && State.wakeSecondsRemaining <= e.secondsRemaining)
                {
                    e.hasTriggered = true;
                    OnScheduledEvent?.Invoke(e);
                }
            }

            // when 30 seconds are remaining in wake state play a warning that you are about to boot down
            if (!_warningPlayed && State.wakeSecondsRemaining <= 30f)
            {
                Debug.Log("warningPlayed");
                if (sleepWarning != null)
                {
                    sleepWarning.Play();
                }
                _warningPlayed = true;
            }

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

        // Reset triggers for the new year so they can fire again if scheduled
        foreach (var e in scheduledEvents)
            if (e.year == State.currentYear)
                e.hasTriggered = false;
    }

    void StartSleepCycle()
    {

        State.wakePhase = WakePhase.Sleeping;
        State.sleepSecondsRemaining = sleepDurationSeconds;

    }
}