using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;


public class SprayController : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;
    public SteamVR_Action_Boolean grabAction;
    private Interactable interactable;
    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    // Update is called once per frame
    void Update()
    {

        if ((interactable.attachedToHand != null) && grabAction.GetState(SteamVR_Input_Sources.Any))
        {
            particles.Play();
        }
        
    }
}
