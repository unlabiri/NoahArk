using System.Collections;
using UnityEngine;

public class SleepCutsceneManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WakeCycleManager wakeCycleManager;
    [SerializeField] private Transform cameraRig;       // Drag in: Player
    [SerializeField] private Transform mapCenter;       // Drag in: your map center empty GameObject
    [SerializeField] private SleepCutsceneUI cutsceneUI;

    public AudioSource bootingDown;
    public AudioSource endSong;

    [Header("Locomotion")]
    [SerializeField] private MonoBehaviour[] locomotionComponents; // PlayerMovement, Snap Turn

    private void OnEnable()
    {
        wakeCycleManager.OnStateChange += HandleStateChange;
    }

    private void OnDisable()
    {
        wakeCycleManager.OnStateChange -= HandleStateChange;
    }


    private void HandleStateChange(WakePhase phase)
    {
        if (phase == WakePhase.Sleeping)
            StartCoroutine(SleepSequence());
        else if (phase == WakePhase.Completed)
        {
            StartCoroutine(EndingSequence());
        }

    }

    private IEnumerator SleepSequence()
    {
        SetLocomotion(false);
        if (bootingDown != null)
        {
            bootingDown.Play();
        }
        yield return cutsceneUI.FadeOut(1.5f);

        TeleportToCenter();

        yield return cutsceneUI.ShowStats();

        yield return cutsceneUI.FadeIn(1.5f);

        SetLocomotion(true);
    }

    private IEnumerator EndingSequence()
    {

        if ( endSong != null)
        {
            endSong.Play();
        }
        SetLocomotion(false);
        yield return cutsceneUI.FadeOut(1.5f);

        TeleportToCenter();

        yield return cutsceneUI.ShowEnding();

        
    }

    private void TeleportToCenter()
    {
        Camera steamVRCam = cameraRig.GetComponentInChildren<Camera>();
        Vector3 headOffset = steamVRCam.transform.position - cameraRig.position;
        headOffset.y = 0f;

        Vector3 targetPosition = mapCenter.position - headOffset;

        CharacterController cc = cameraRig.GetComponent<CharacterController>();
        VRPlayerMovement pm = cameraRig.GetComponent<VRPlayerMovement>();

        if (cc != null) cc.enabled = false;

        cameraRig.position = targetPosition;
        cameraRig.rotation = Quaternion.Euler(0f, mapCenter.eulerAngles.y, 0f);

        // Reset accumulated velocity so player doesn't fall through on re-enable
        if (pm != null) pm.ResetVerticalVelocity();

        if (cc != null) cc.enabled = true;
    }

    private void SetLocomotion(bool enabled)
    {
        foreach (var comp in locomotionComponents)
            if (comp != null) comp.enabled = enabled;
    }
}