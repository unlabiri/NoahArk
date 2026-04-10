using UnityEngine;

public class FaultRandomizer : MonoBehaviour
{
    [Header("Randomness Settings")]
    [Tooltip("Minimum seconds before trying to trigger a fault.")]
    public float minCheckInterval = 10f;
    [Tooltip("Maximum seconds before trying to trigger a fault.")]
    public float maxCheckInterval = 30f;
    [Tooltip("Percentage chance (0-100) that a fault actually happens when the timer goes off.")]
    [Range(0, 100)] public int faultChance = 50;

    [Header("The Fault to Break")]
    public WiringBox wiringBox;

    private float timer;
    private float currentInterval;

    void Start()
    {
        SetNextInterval();
    }

    void Update()
    {
        // Only run the timer if the box isn't already broken
        if (wiringBox != null && !wiringBox.isBroken)
        {
            timer += Time.deltaTime;

            if (timer >= currentInterval)
            {
                RollTheDice();
                SetNextInterval();
            }
        }
    }

    void RollTheDice()
    {
        int roll = Random.Range(0, 100);
        if (roll < faultChance)
        {
            wiringBox.TriggerFault();
        }
    }

    void SetNextInterval()
    {
        timer = 0f;
        currentInterval = Random.Range(minCheckInterval, maxCheckInterval);
    }
}