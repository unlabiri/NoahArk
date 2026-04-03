using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    [Header("Door to Move")]
    [SerializeField] private Transform doorVisual;

    [Header("Slide Settings")]
    [SerializeField] private Vector3 openOffset = new Vector3(1.2f, 0f, 0f);
    [SerializeField] private float speed = 2.5f;

    [Header("Detection")]
    [SerializeField] private string playerTag = "Player";

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool shouldOpen;
    private int bodiesInside = 0;

    private void Start()
    {
        if (doorVisual == null)
        {
            Debug.LogError("SlidingDoor: doorVisual is not assigned.", this);
            enabled = false;
            return;
        }

        closedPosition = doorVisual.position;
        openPosition = closedPosition + openOffset;

        Debug.Log("SlidingDoor started on " + gameObject.name);
    }

    private void Update()
    {
        Vector3 targetPosition = shouldOpen ? openPosition : closedPosition;

        doorVisual.position = Vector3.MoveTowards(
            doorVisual.position,
            targetPosition,
            speed * Time.deltaTime
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered trigger: " + other.name);

        if (!other.CompareTag(playerTag))
        {
            Debug.Log("Entered, but tag was not Player");
            return;
        }

        Debug.Log("Player entered trigger");
        bodiesInside++;
        shouldOpen = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Something exited trigger: " + other.name);

        if (!other.CompareTag(playerTag))
            return;

        Debug.Log("Player exited trigger");
        bodiesInside = Mathf.Max(0, bodiesInside - 1);

        if (bodiesInside == 0)
            shouldOpen = false;
    }
}