using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;

    [Header("Step Detection")]
    public float stepCooldown = 0.4f;
    public float sprintStepCooldown = 0.25f; // faster cadence when sprinting

    [Range(0f, 1f)] public float volumeMin = 0.3f;
    [Range(0f, 1f)] public float volumeMax = 0.6f;

    private FootstepSurface currentSurface;
    private float lastStepTime;
    public bool onSurface = false;

    // Called each frame by VRPlayerMovement
    public void OnMovementUpdate(float speed, bool isSprinting, bool isGrounded)
    {
        if (!onSurface || currentSurface == null) return;
        if (!isGrounded) return;
        if (currentSurface.footstepClips == null || currentSurface.footstepClips.Length == 0) return;

        float cooldown = isSprinting ? sprintStepCooldown : stepCooldown;

        if (speed > 0.1f && Time.time - lastStepTime >= cooldown)
        {
            PlayStep();
        }
    }

    void PlayStep()
    {
        lastStepTime = Time.time;
        AudioClip[] clips = currentSurface.footstepClips;
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.volume = Random.Range(volumeMin, volumeMax);
        audioSource.PlayOneShot(clip);
    }

    public void OnEnterSurface(FootstepSurface surface)
    {
        if (currentSurface == surface) return;
        currentSurface = surface;
        onSurface = true;
        Debug.Log("Entered surface on: " + surface.gameObject.name, surface.gameObject);
    }

    public void OnExitSurface(FootstepSurface surface)
    {
        if (currentSurface == surface)
        {
            currentSurface = null;
            onSurface = false;
        }
    }
}