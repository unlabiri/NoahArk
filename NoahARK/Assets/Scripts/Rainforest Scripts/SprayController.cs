using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;


public class SprayController : MonoBehaviour
{

    public AudioSource spraySound;

    [SerializeField] private ParticleSystem particles;
    public SteamVR_Action_Boolean grabAction;
    private Interactable interactable;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<Interactable>();
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    private void OnDetachedFromHand(Hand hand)
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        particles.Stop();
    }

    // Update is called once per frame
    void Update()
    {

        if ((interactable.attachedToHand != null) && grabAction.GetState(SteamVR_Input_Sources.Any))
        {
            particles.Play();
            spraySound.Play();
        }
        
    }
}
