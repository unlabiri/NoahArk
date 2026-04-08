using UnityEngine;

public class VRWireSocket : MonoBehaviour
{
    public WireColor requiredColor;
    public WiringBox mainBox; // Drag your grey square here

    private bool hasWireInIt = false;

    void OnTriggerEnter(Collider other)
    {
        // Did a wire just touch this socket?
        VRWirePlug plug = other.GetComponent<VRWirePlug>();

        if (plug != null && !hasWireInIt && !plug.isPluggedIn)
        {
            // Do the colors match?
            if (plug.myColor == requiredColor)
            {
                hasWireInIt = true;
                plug.LockIntoSocket(this.transform);
                mainBox.ConnectCorrectWire(); // Tell the main square we fixed one!
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        VRWirePlug plug = other.GetComponent<VRWirePlug>();
        if (plug != null && plug.myColor == requiredColor && hasWireInIt)
        {
            hasWireInIt = false;
            mainBox.DisconnectWire();
        }
    }
}