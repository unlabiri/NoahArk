using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquaFaultManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WakeCycleManager wakeCycle;

    [Header("Fault Pool — drag all your fault GameObjects here")]
    [SerializeField] private List<AquaFaultBase> faultPool = new();

    [Header("Settings")]
    [SerializeField] private int faultsPerDay = 2;

    [Header("Audio")]
    [SerializeField] private AudioSource faultAlertSource;
    [SerializeField] private AudioClip faultAlertClip;
    [SerializeField] private float alertDelay = 10f;

    private List<AquaFaultBase> activeFaults = new();
    private bool alertPlayedThisCycle = false;  // gates it to once per day

    private void OnEnable()
    {
        if (wakeCycle == null) return;
        wakeCycle.OnScheduledEvent += HandleScheduledEvent;
        wakeCycle.OnStateChange += HandleStateChange;
    }

    private void OnDisable()
    {
        if (wakeCycle == null) return;
        wakeCycle.OnScheduledEvent -= HandleScheduledEvent;
        wakeCycle.OnStateChange -= HandleStateChange;
    }

    private void HandleScheduledEvent(WakeCycleScheduledEvent e)
    {
        Debug.Log($"[AquaFaultManager] Scheduled event fired — spawning faults.");
        SpawnFaults();
    }

    private void HandleStateChange(WakePhase phase)
    {
        if (phase != WakePhase.Sleeping) return;

        foreach (var fault in activeFaults)
        {
            if (fault == null) continue;
            if (fault.state != AquaFaultBase.FaultState.Resolved)
                Debug.Log($"[AquaFaultManager] {fault.name} was not resolved this day.");
        }

        activeFaults.Clear();
        alertPlayedThisCycle = false; // reset for the next day
    }

    private void SpawnFaults()
    {
        var available = new List<AquaFaultBase>(faultPool);

        for (int i = 0; i < faultsPerDay; i++)
        {
            if (available.Count == 0) break;

            int index = Random.Range(0, available.Count);
            var fault = available[index];
            available.RemoveAt(index);

            fault.Activate();
            activeFaults.Add(fault);
            Debug.Log($"[AquaFaultManager] Activated: {fault.name}");
        }

        // Only start the alert coroutine once per day, on the first spawn batch
        if (!alertPlayedThisCycle && activeFaults.Count > 0)
        {
            alertPlayedThisCycle = true;
            StartCoroutine(PlayFaultAlertDelayed());
        }
    }

    private IEnumerator PlayFaultAlertDelayed()
    {
        yield return new WaitForSeconds(alertDelay);

        if (faultAlertSource != null && faultAlertClip != null)
        {
            faultAlertSource.PlayOneShot(faultAlertClip);
            Debug.Log("[AquaFaultManager] Fault alert audio played.");
        }
        else
        {
            Debug.LogWarning("[AquaFaultManager] faultAlertSource or faultAlertClip is not assigned!");
        }
    }
}