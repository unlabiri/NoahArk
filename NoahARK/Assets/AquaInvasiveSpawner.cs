using System.Collections.Generic;
using UnityEngine;

public class AquaInvasiveSpawner : MonoBehaviour
{
    [Header("Fault Reference")]
    [SerializeField] private AquaInvasionFault invasionFault;

    [Header("Spawning")]
    [SerializeField] private GameObject invasivePrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int spawnCount = 1;

    private List<GameObject> activeInvasives = new();
    private AquaFaultBase.FaultState lastFaultState;

    private void Update()
    {
        if (invasionFault == null) return;

        var current = invasionFault.state;

        if (current == AquaFaultBase.FaultState.Active &&
            lastFaultState != AquaFaultBase.FaultState.Active)
            SpawnInvasives();

        if (current != AquaFaultBase.FaultState.Active &&
            lastFaultState == AquaFaultBase.FaultState.Active)
            DespawnAll();

        lastFaultState = current;
    }

    private void SpawnInvasives()
    {
        DespawnAll();

        for (int i = 0; i < spawnCount; i++)
        {
            var point = spawnPoints[i % spawnPoints.Length];
            var obj = Instantiate(invasivePrefab, point.position, point.rotation);
            obj.tag = "InvasiveSpecies";
            activeInvasives.Add(obj);
            Debug.Log($"[Spawner] Invasive spawned at {point.name}");
        }
    }

    public void DespawnAll()
    {
        foreach (var inv in activeInvasives)
            if (inv != null) Destroy(inv);
        activeInvasives.Clear();
    }

    public void NotifyCapture(GameObject captured)
    {
        activeInvasives.Remove(captured);
    }
}