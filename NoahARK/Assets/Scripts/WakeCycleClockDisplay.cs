using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class WakeCycleClockDisplay : MonoBehaviour
{
    [SerializeField] private WakeCycleManager wakeCycle;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text phaseText;


    private void Reset()
    {
        wakeCycle = FindObjectOfType<WakeCycleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (wakeCycle == null || wakeCycle.State == null) return;

        var s = wakeCycle.State;

        float secondsRemaining;
        string label;
        int year = s.currentYear;

        switch (s.wakePhase)
        {
            case WakePhase.Awake:
                secondsRemaining = s.wakeSecondsRemaining;
                label = "AWAKE";
                break;
            case WakePhase.Sleeping:
                secondsRemaining = s.sleepSecondsRemaining;
                label = "SLEEP";
                break;
            case WakePhase.Completed:
                secondsRemaining = 0f;
                label = "COMPLETED";
                break;
            default:
                secondsRemaining=0f;
                label = "DEFAULT";
                break;
        }

        if(timeText != null)
        {
            string time = FormatMMSS(secondsRemaining);
            timeText.text = $"year {year} {label} {time}";
            

        }
    }

    private static string FormatMMSS(float seconds)
    {
        seconds = Mathf.Max(0f, seconds);
        int total = Mathf.CeilToInt(seconds);
        int mm = total / 60;
        int ss = total % 60;
        return $"{mm: 00}:{ss:00}";
    }
}
