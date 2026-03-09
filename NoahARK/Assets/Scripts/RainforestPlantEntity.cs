using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainforestPlantEntity : MonoBehaviour
{
    public RainforestPlantState State = new RainforestPlantState();
    [SerializeField] private float maxSafeTemperature;
    [SerializeField] private float minSafeTemperature;
    [SerializeField] private float maxSafeHumidity = .88f;
    [SerializeField] private float minSafeHumidity = .77f;

    [SerializeField] private float secondsToReduceHealth = 10f; //240 real time
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
