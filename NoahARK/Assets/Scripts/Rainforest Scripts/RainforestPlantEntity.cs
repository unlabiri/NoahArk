using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.VisualBasic;
using UnityEngine;

public class RainforestPlantEntity : MonoBehaviour
{
    public RainforestPlantState plantState = new RainforestPlantState();
    public RainforestBiomeController biomeController;

    [SerializeField] private float maxSafeTemperature = 50f;
    [SerializeField] private float minSafeTemperature = 100f;
    [SerializeField] private float maxSafeHumidity = .88f;
    [SerializeField] private float minSafeHumidity = .77f;

    [SerializeField] private float damageIntervalTemperature = 10f; //180 real time (3 min)
    [SerializeField] private float damageIntervalHumidity = 10f; //240 real time (4 min)
    public void Initialize(RainforestBiomeController controller)
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
        if (plantState.health == 0) return;
        var biomeState = biomeController.State;
        if (!plantState.isAlive || biomeController == null)
            return;
        bool tempUnsafe = biomeState.temperature > maxSafeTemperature
        || biomeState.temperature < minSafeTemperature;


        if (tempUnsafe)
        {
            plantState.timeInUnsafeTemperature += Time.deltaTime;
        }

        if ( tempUnsafe && 
        (plantState.timeInUnsafeTemperature >= damageIntervalTemperature) 
        // && (plantState.health > 0)
        )
        {
            plantState.health -= 20;
            UpdatePlantLifeStage();

            Debug.Log(plantState.health);
            Debug.Log(plantState.stage);

            damageIntervalTemperature += 10f;
        }

        if (!tempUnsafe)
        {
            // reset the timers when the temp is stabilized and begin slow regeneration
            plantState.timeInUnsafeTemperature = 0f;
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
}
