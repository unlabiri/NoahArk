using UnityEngine;

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

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isOpen;

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

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryToggleDoor();
        }
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        doorPivot.localRotation = Quaternion.Slerp(
            doorPivot.localRotation,
            targetRotation,
            Time.deltaTime * openSpeed
        );
    }

    public void TryToggleDoor()
    {
        if (locked)
        {
            Debug.Log("Door is locked.");
            return;
        }

        isOpen = !isOpen;
    }

    public void OpenDoor()
    {
        if (locked) return;
        isOpen = true;
    }

    public void CloseDoor()
    {
        isOpen = false;
    }

    public void LockDoor()
    {
        locked = true;
    }

    public void UnlockDoor()
    {
        locked = false;
    }

    public bool IsOpen()
    {
        return isOpen;
    }

    public bool IsLocked()
    {
        return locked;
    }
}