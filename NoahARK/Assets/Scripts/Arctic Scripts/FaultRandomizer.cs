using UnityEngine;

public class FaultRandomizer : MonoBehaviour
{
    [Header("Randomness Settings")]
    public float minCheckInterval = 10f;
    public float maxCheckInterval = 30f;
    [Range(0, 100)] public int faultChance = 50;

    [Header("The Ship Faults")]
    public WiringBox wiringBox;
    public TemperatureFault tempPipe; // NEW: Slot for your AC pipe

    private float timer;
    private float currentInterval;

    void Start()
    {
        SetNextInterval();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentInterval)
        {
            RollTheDice();
            SetNextInterval();
        }
    }

    void RollTheDice()
    {
        if (Random.Range(0, 100) < faultChance)
        {
            // Flip a coin (0 or 1) to pick which system breaks
            int whichFault = Random.Range(0, 2);

            if (whichFault == 0 && wiringBox != null && !wiringBox.isBroken)
            {
                wiringBox.TriggerFault();
            }
            else if (whichFault == 1 && tempPipe != null && !tempPipe.isBroken)
            {
                tempPipe.TriggerFault();
            }
        }
    }

    void SetNextInterval()
    {
        timer = 0f;
        currentInterval = Random.Range(minCheckInterval, maxCheckInterval);
    }
}