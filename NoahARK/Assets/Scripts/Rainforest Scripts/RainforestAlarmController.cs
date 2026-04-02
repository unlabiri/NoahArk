using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainforestAlarmController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public RainforestBiomeController biomeController;
    [SerializeField] private Renderer alarmRenderer;

    [SerializeField] private Material healthyAlarmMaterial;
    [SerializeField] private Material vulnerableAlarmMaterial;
    [SerializeField] private Material endangeredAlarmmaterial;

    [SerializeField] private Light alarmLight;
    BiomeHealthState lastAppliedStage;

    public RainforestBiomeState biomeState;


    void Start()
    {
        biomeState = biomeController.State;
        ApplyVisualState();
    }

    // Update is called once per frame
    void Update()
    {
        if(biomeState.health != lastAppliedStage)
        {
            ApplyVisualState();
        }
        
    }

    private void ApplyVisualState()
    {
        var stage = biomeState.health;
        lastAppliedStage = stage;

        switch(stage)
        {
            case BiomeHealthState.Healthy:
                alarmRenderer.material = healthyAlarmMaterial;
                alarmLight.color = Color.green;
                break;
            case BiomeHealthState.Vulnerable:
                alarmRenderer.material = vulnerableAlarmMaterial;
                alarmLight.color = Color.yellow;
                break;
            case BiomeHealthState.Endangered:
                alarmRenderer.material = endangeredAlarmmaterial;
                alarmLight.color = Color.red;
                break;
            case BiomeHealthState.Extinct:
                alarmRenderer.material = endangeredAlarmmaterial;
                alarmLight.color = Color.black;
                break;
            default:
                alarmRenderer.material = healthyAlarmMaterial;
                alarmLight.color = Color.black;
                break;


        }
    }
}
