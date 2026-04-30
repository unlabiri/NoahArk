using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainforestFaultManager : MonoBehaviour
{
    // Start is called before the first frame update
    public WakeCycleManager wakeCycleManager;
    public RainforestBiomeController biomeController;
    private RainforestBiomeState biomeState;

    public AudioSource faultAnnouncement;

    void Start()
    {
        if (biomeController != null)
        {
            biomeState = biomeController.State;
        }

        wakeCycleManager.OnScheduledEvent += HandleFaultTrigger;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerTemperatureFault()
    {
        
        int roll = Random.Range(0, 2); // 0 or 1, pick a side
        int result = 0;

        if (roll == 0)
            result = Random.Range(0, 51);    // 0–50
        else
            result = Random.Range(101, 151); // 101–150

        biomeState.temperature = result;
        Debug.Log("Temperature fault" + result);
    }
    public void TriggerHumidityFault()
    {
        int roll = Random.Range(0, 2); // 0 or 1, pick a side
        float result = 0f;

        if (roll == 0)
            result = Random.Range(0, 0.77f);    // 0–50
        else
            result = Random.Range(.89f, 1f); // 101–150

        biomeState.humidity = result;

        Debug.Log("humidity fault" + result);
    }
    public void TriggerInvasiveSpeciesFault()
    {
        Debug.Log("invasive fault");

        int infectedPlantIndex = Random.Range(0, biomeController.plants.Count);

        biomeController.plants[infectedPlantIndex].plantState.isInfected = true;

    }
    public void HandleFaultTrigger(WakeCycleScheduledEvent e)
    {
        if (e.targetBiome != "Rainforest") return;
        int roll = Random.Range(1, 4); // 1, 2, or 3 — note Unity's Range is exclusive on the max

        // when a fault is triggered play an annoucement
        faultAnnouncement.Play();

        if (roll == 1)
            TriggerTemperatureFault();
        else if (roll == 2)
            TriggerHumidityFault();
        else
            TriggerInvasiveSpeciesFault();

    }
}
