using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
    // Adjust this value in the Inspector to control speed (e.g., 0.5f or 1.2f)
    public float rotationSpeed = 1.2f;

    void Update()
    {
        // Sets the rotation based on elapsed time and your speed variable
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed);
    }
}
