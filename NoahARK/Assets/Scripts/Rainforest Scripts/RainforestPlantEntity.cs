using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.VisualBasic;
using UnityEngine;
using System;

public class RainforestPlantEntity : MonoBehaviour
{
    public RainforestPlantState plantState;
    public RainforestBiomeController biomeController;

    [SerializeField] private float maxSafeTemperature = 100f;
    [SerializeField] private float minSafeTemperature = 50f;
    [SerializeField] private float maxSafeHumidity = .88f;
    [SerializeField] private float minSafeHumidity = .77f;

    [SerializeField] private float damageIntervalTemperature = 10f; //180 real time (3 min)
    [SerializeField] private float damageIntervalHumidity = 10f; //240 real time (4 min)
    [SerializeField] private float damageIntervalInfection = 10f;
    [SerializeField] private float propagateInfectionTime = 25f;

    private List<RainforestPlantEntity> nearbyPlants = new List<RainforestPlantEntity>();

    public void Initialize(RainforestBiomeController controller)
    {
        biomeController = controller;
    }
    // Start is called before the first frame update
    private void Start()
    {
        plantState = new RainforestPlantState();
        if (biomeController == null)
        {
            biomeController = FindObjectOfType<RainforestBiomeController>();
        }

        biomeController?.RegisterPlant(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (plantState.health == 0) return;
        var biomeState = biomeController.State;
        if (!plantState.isAlive || biomeController == null)
            return;
        bool tempUnsafe = (biomeState.temperature > maxSafeTemperature) || (biomeState.temperature < minSafeTemperature);
        
        bool humidityUnsafe = biomeState.humidity > maxSafeHumidity || biomeState.humidity < minSafeHumidity;

        
        if (tempUnsafe)
        {
            plantState.timeInUnsafeTemperature += Time.deltaTime;
        }

        if (humidityUnsafe)
        {
            plantState.timeInUnsafeHumidity += Time.deltaTime; 
        }

        if (plantState.isInfected)
        {
            plantState.timeInfected += Time.deltaTime;
        }
        // triggers infeciton propagation

        if (plantState.isInfected &&
            (plantState.timeInfected >= propagateInfectionTime))
        {
            foreach(RainforestPlantEntity plant in nearbyPlants)
            {
                if (plant != null)
                {
                    plant.plantState.isInfected = true;
                    
                }
            }


        }

        if (plantState.isInfected &&
            (plantState.timeInfected >= damageIntervalInfection) &&
            (plantState.health > 0))
        {
            plantState.health -= 10;
            UpdatePlantLifeStage();
            damageIntervalInfection += 10f;
        }

        if (humidityUnsafe &&
            (plantState.timeInUnsafeHumidity >= damageIntervalHumidity) &&
            (plantState.health > 0)) 
        {
            plantState.health -= 20;
            UpdatePlantLifeStage();

            Debug.Log(humidityUnsafe);

            damageIntervalHumidity += 10f;

        }
        if ( tempUnsafe && 
        (plantState.timeInUnsafeTemperature >= damageIntervalTemperature) 
         && (plantState.health > 0)
        )
        {
            plantState.health -= 20;
            UpdatePlantLifeStage();

            Debug.Log(plantState.health);
            Debug.Log(plantState.stage);

            damageIntervalTemperature += 10f;
        }

        if (!tempUnsafe && !humidityUnsafe && !plantState.isInfected)
        {
            // reset the timers when the temp is stabilized and begin slow regeneration
            plantState.timeInUnsafeTemperature = 0f;
            plantState.timeInUnsafeHumidity = 0f;
            plantState.timeInfected = 0f;
            if (plantState.health < 100)
            {
                plantState.health += 10;
                UpdatePlantLifeStage();
                if (plantState.health >= 100)
                {
                    plantState.health = 100;
                }
            }
            
        }
    }
    public void UpdatePlantLifeStage()
    {
        if (plantState.health <= 0)
        {
            plantState.stage = PlantStage.Dead;
            plantState.isAlive = false;
        } else if (plantState.health >= 1 && plantState.health <= 49)
        {
            plantState.stage = PlantStage.Wilted2;
        } else if (plantState.health >= 50 && plantState.health <= 89)
        {
            plantState.stage = PlantStage.Wilted1;
        } else
        {
            plantState.stage = PlantStage.Healthy;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        RainforestPlantEntity plant = other.GetComponent<RainforestPlantEntity>();

        if (plant != null && plant != this)
        {
            nearbyPlants.Add(plant);
        }
    }
}
