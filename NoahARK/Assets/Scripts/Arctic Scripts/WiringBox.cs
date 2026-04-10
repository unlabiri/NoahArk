using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic; // Required for making Lists!

public class WiringBox : MonoBehaviour
{
    [Header("Wiring Box Status")]
    public bool isBroken = false;
    public int totalWiresToFix = 4;
    private int currentlyFixedWires = 0;

    // NEW: A list to hold the physical sockets so we can move them
    [Header("Sockets (For Shuffling)")]
    public Transform[] physicalSockets;

    [Header("Events (Hook up sounds/lights here)")]
    public UnityEvent onBoxBroken;
    public UnityEvent onBoxFixed;

    public void TriggerFault()
    {
        if (isBroken) return;

        isBroken = true;
        currentlyFixedWires = 0;

        ShuffleSockets(); // NEW: Shuffle the holes instantly before the wires pop out!

        Debug.Log("Wiring Box Fault Occurred! Wires popped out.");
        onBoxBroken.Invoke();
    }

    // NEW: The shuffling math
    void ShuffleSockets()
    {
        if (physicalSockets.Length == 0) return; // Failsafe just in case you forgot to add them

        // 1. Copy down all 4 current positions
        List<Vector3> originalPositions = new List<Vector3>();
        for (int i = 0; i < physicalSockets.Length; i++)
        {
            // We use localPosition so it stays attached to the wall correctly
            originalPositions.Add(physicalSockets[i].localPosition);
        }

        // 2. Shuffle the copied positions
        for (int i = 0; i < originalPositions.Count; i++)
        {
            Vector3 temp = originalPositions[i];
            int randomIndex = Random.Range(i, originalPositions.Count);
            originalPositions[i] = originalPositions[randomIndex];
            originalPositions[randomIndex] = temp;
        }

        // 3. Teleport the sockets to the newly shuffled positions
        for (int i = 0; i < physicalSockets.Length; i++)
        {
            physicalSockets[i].localPosition = originalPositions[i];
        }
    }

    public void ConnectCorrectWire()
    {
        if (!isBroken) return;

        currentlyFixedWires++;

        // UPDATE THIS LINE to include the Instance ID:
        Debug.Log($"Wire fixed! {currentlyFixedWires}/{totalWiresToFix} connected. (Brain ID: {this.gameObject.GetInstanceID()})");

        if (currentlyFixedWires >= totalWiresToFix)
        {
            isBroken = false;
            Debug.Log("Wiring Box completely repaired.");
            onBoxFixed.Invoke();
        }
    }

    public void DisconnectWire()
    {
        if (currentlyFixedWires > 0)
        {
            currentlyFixedWires--;

            // ADD THIS LINE:
            Debug.Log($"Physics jitter! Wire disconnected. Count dropped to: {currentlyFixedWires}");
        }
    }
}