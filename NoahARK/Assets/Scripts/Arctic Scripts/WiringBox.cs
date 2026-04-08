using UnityEngine;
using UnityEngine.Events;

public class WiringBox : MonoBehaviour
{
    [Header("Wiring Box Status")]
    public bool isBroken = false;
    public int totalWiresToFix = 4;
    private int currentlyFixedWires = 0;

    [Header("Events (Hook up sounds/lights here)")]
    public UnityEvent onBoxBroken;
    public UnityEvent onBoxFixed;

    public void TriggerFault()
    {
        if (isBroken) return;

        isBroken = true;
        currentlyFixedWires = 0;

        Debug.Log("Wiring Box Fault Occurred! Wires popped out.");
        onBoxBroken.Invoke();
    }

    public void ConnectCorrectWire()
    {
        if (!isBroken) return;

        currentlyFixedWires++;
        Debug.Log($"Wire fixed! {currentlyFixedWires}/{totalWiresToFix} connected.");

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
        }
    }
}