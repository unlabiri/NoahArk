using UnityEngine;
using Valve.VR.InteractionSystem;

public class CageDoorController : MonoBehaviour
{
    [Header("Door Pivot")]
    [SerializeField] private Transform doorPivot;

    [Header("Door Settings")]
    [SerializeField] private float openAngle = -90f;
    [SerializeField] private float openSpeed = 2f;

    [Header("State")]
    [SerializeField] private bool startsOpen = false;
    [SerializeField] private bool locked = false;

    [Header("VR Proximity")]
    [Tooltip("Tag applied to both SteamVR hand GameObjects")]
    [SerializeField] private string handTag = "Hand";
    [Tooltip("Close the door again after hand leaves")]
    [SerializeField] private bool closeOnHandExit = false;

    [Header("Feedback")]
    [SerializeField] private AudioSource openSound;
    [SerializeField] private AudioSource closeSound;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isOpen;
    private int handsNearby = 0; // track both hands independently

    private void Start()
    {
        if (doorPivot == null)
        {
            Debug.LogError($"{name}: Door pivot is missing.");
            enabled = false;
            return;
        }

        closedRotation = doorPivot.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);
        isOpen = startsOpen;
        doorPivot.localRotation = isOpen ? openRotation : closedRotation;
    }

    private void Update()
    {
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        doorPivot.localRotation = Quaternion.Slerp(
            doorPivot.localRotation,
            targetRotation,
            Time.deltaTime * openSpeed
        );
    }

    // ── VR Proximity ─────────────────────────────────────────────────────────

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Hand>() == null) return;

        handsNearby++;
        OpenDoor();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Hand>() == null) return;

        handsNearby = Mathf.Max(0, handsNearby - 1);

        if (closeOnHandExit && handsNearby == 0)
            CloseDoor();
    }
    // ── Public API (unchanged — anything else in your project can still call these) ──

    public void TryToggleDoor()
    {
        if (locked) { Debug.Log("Door is locked."); return; }
        isOpen = !isOpen;
    }

    public void OpenDoor()
    {
        if (locked) return;
        if (!isOpen)
        {
            isOpen = true;
            if (openSound != null) openSound.Play();
        }
    }

    public void CloseDoor()
    {
        if (isOpen)
        {
            isOpen = false;
            if (closeSound != null) closeSound.Play();
        }
    }

    public void LockDoor() => locked = true;
    public void UnlockDoor() => locked = false;
    public bool IsOpen() => isOpen;
    public bool IsLocked() => locked;
}