using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class FootstepSurface : MonoBehaviour
{
    public string surfaceTag = "Default";
    public FootstepManager footstepManager;

    public AudioClip[] footstepClips;

    void Awake()
    {
        // Make sure the collider is a trigger
        GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (footstepManager != null)
            footstepManager.OnEnterSurface(this);
    }

    void OnTriggerStay(Collider other)
    {
        // Only fires if we haven't registered yet (player spawned inside)
        if (footstepManager != null && !footstepManager.onSurface)
            footstepManager.OnEnterSurface(this);
    }


    void OnTriggerExit(Collider other)
    {
        if (footstepManager != null)
            footstepManager.OnEnterSurface(this);
    }
}