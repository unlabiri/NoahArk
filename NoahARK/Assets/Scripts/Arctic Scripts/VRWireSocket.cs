using UnityEngine;

public class VRWireSocket : MonoBehaviour
{
    public WireColor requiredColor;
    public WiringBox mainBox;

    private bool hasWireInIt = false;

    // NEW: Auto-color the socket to match its required wire
    void Start()
    {
        Renderer rend = GetComponent<Renderer>();

        switch (requiredColor)
        {
            case WireColor.Red: rend.material.color = Color.red; break;
            case WireColor.Blue: rend.material.color = Color.blue; break;
            case WireColor.Yellow: rend.material.color = Color.yellow; break;
            case WireColor.Green: rend.material.color = Color.green; break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        VRWirePlug plug = other.GetComponent<VRWirePlug>();

        if (plug != null && !hasWireInIt && !plug.isPluggedIn)
        {
            if (plug.myColor == requiredColor)
            {
                hasWireInIt = true;
                plug.LockIntoSocket(this.transform);
                mainBox.ConnectCorrectWire();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        VRWirePlug plug = other.GetComponent<VRWirePlug>();

        if (plug != null && plug.myColor == requiredColor && hasWireInIt)
        {
            // NEW JITTER FIX: If the plug still thinks it's locked in, ignore this fake physics exit!
            if (plug.isPluggedIn)
            {
                return;
            }

            hasWireInIt = false;
            mainBox.DisconnectWire();
        }
    }
}