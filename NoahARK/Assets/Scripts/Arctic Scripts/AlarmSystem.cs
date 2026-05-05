using UnityEngine;

public class AlarmSystem : MonoBehaviour
{
    private AudioSource alarmAudio;
    private int activeFaults = 0; // Tracks how many things are currently broken

    void Awake()
    {
        alarmAudio = GetComponent<AudioSource>();
    }

    // Call this when ANY fault starts
    public void TurnOnAlarm()
    {
        activeFaults++;

        // Only press play if it isn't already playing
        if (!alarmAudio.isPlaying)
        {
            alarmAudio.Play();
            Debug.Log("<color=red>[ALARM]</color> Warning! Fault detected!");
        }
    }

    // Call this when ANY fault is fixed
    public void TurnOffAlarm()
    {
        activeFaults--;

        // Safety net just in case it drops below 0
        if (activeFaults < 0) activeFaults = 0;

        // ONLY turn off the alarm if ALL faults are fixed
        if (activeFaults == 0)
        {
            alarmAudio.Stop();
            Debug.Log("<color=green>[ALARM]</color> All systems nominal. Alarm stopped.");
        }
    }
}