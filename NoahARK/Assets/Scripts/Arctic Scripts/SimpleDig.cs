using UnityEngine;
using UnityEngine.Events;

public class SimpleDig : MonoBehaviour
{
    [Header("Fault Status")]
    public bool isBroken = false;
    public int hitsToClear = 10;
    private int currentHits = 0;

    [Header("Cooldown Settings")]
    public float digCooldown = 2.0f; // Only 1 dig every 2 seconds
    private float nextAllowedDigTime = 0f;

    [Header("References")]
    public GameObject airVent; // The vent hidden under the snow
    public UnityEvent onVentCleared;
    public UnityEvent onFaultTriggered;

    private Vector3 originalScale;

    void Awake()
    {
        originalScale = transform.localScale;
        // Start with the snow pile inactive
        gameObject.SetActive(false);
    }

    public void TriggerFault()
    {
        if (isBroken) return;

        isBroken = true;
        currentHits = 0;
        nextAllowedDigTime = 0f; // Reset cooldown

        gameObject.SetActive(true);
        transform.localScale = originalScale;

        onFaultTriggered.Invoke();

        Debug.Log("<color=red>[Vent Fault]</color> Air vent blocked by snow! Start digging!");
    }

    void OnTriggerEnter(Collider other)
    {
        // 1. Is the fault active? 2. Is it the shovel? 3. Has 2 seconds passed?
        if (isBroken && other.CompareTag("Shovel"))
        {
            if (Time.time >= nextAllowedDigTime)
            {
                ProcessDig();
            }
            else
            {
                float timeLeft = nextAllowedDigTime - Time.time;
                Debug.Log($"<color=yellow>[SimpleDig]</color> Too fast! Wait {timeLeft:F1}s");
            }
        }
    }

    void ProcessDig()
    {
        currentHits++;
        nextAllowedDigTime = Time.time + digCooldown; // Set the 2-second timer

        // Visual feedback: Shrink the pile slightly (1/10th each time)
        transform.localScale *= 0.85f;

        Debug.Log($"<color=cyan>[SimpleDig]</color> Hit {currentHits}/10. Next dig in 2s.");

        if (currentHits >= hitsToClear)
        {
            FinishFault();
        }
    }

    void FinishFault()
    {
        isBroken = false;
        Debug.Log("<color=green>[SimpleDig]</color> Vent cleared! Oxygen/Temp restoring.");

        onVentCleared.Invoke(); // Tell the randomizer or temp script it's fixed
        gameObject.SetActive(false); // Hide the snow pile
    }
}