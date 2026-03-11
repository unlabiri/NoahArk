using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.VisualBasic;
using UnityEngine;

public class RainforestPlantEntity : MonoBehaviour
{
    public RainforestPlantState State = new RainforestPlantState();
    public RainforestBiomeController biomeController;
    public RainforestBiomeState biomeState = biomeController.State;
    [SerializeField] private float maxSafeTemperature = 50f;
    [SerializeField] private float minSafeTemperature = 100f;
    [SerializeField] private float maxSafeHumidity = .88f;
    [SerializeField] private float minSafeHumidity = .77f;

    [SerializeField] private float damageIntervalTemperature = 10f; //180 real time (3 min)
    [SerializeField] private float damageIntervalHumidity = 10f; //240 real time (4 min)
    void Initialize(RainforestBiomeController controller)
    {
        biomeController = controller;
    }
    // Start is called before the first frame update
    private void Start()
    {
        if (biomeController == null)
        {
            biomeController = FindObjectOfType<RainforestBiomeController>();
        }

        biomeController?.RegisterPlant(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (!State.isAlive || biomeController == null)
            return;
        bool tempUnsafe = biomeState.tempUnsafe > maxSafeTemperature
        || biomeState.temperature < minSafeTemperature;

        if (tempUnsafe)
        {
            State.timeInUnsafeTemperature += Time.deltaTime;
        }

        if (tempUnsafe &&
        (State.timeInUnsafeTemperature == damageIntervalTemperature) &&
        State.health > 0)
        {
            State.health -= 20;
            damangeIntervalTemperature += damageIntervalTemperature;
        }
    }
}
