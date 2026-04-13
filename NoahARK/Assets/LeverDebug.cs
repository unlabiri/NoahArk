using UnityEngine;
using Valve.VR.InteractionSystem;

public class LeverDebug : MonoBehaviour
{
    private void OnHandHoverBegin(Hand hand)
    {
        Debug.Log("HOVER BEGIN");
    }

    private void HandHoverUpdate(Hand hand)
    {
        Debug.Log("Hovering - grab: " + hand.GetGrabStarting());
    }
}