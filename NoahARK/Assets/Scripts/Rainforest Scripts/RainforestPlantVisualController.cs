using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RainforestPlantVisualController : MonoBehaviour
{
    [SerializeField] private RainforestPlantEntity plant;

    [SerializeField] private GameObject healthyModel;
    [SerializeField] private GameObject wiltedModel;

    [SerializeField] private Renderer healthyRenderer;
    [SerializeField] private Renderer wiltedRenderer;

    [SerializeField] private Material healthyMaterial;
    [SerializeField] private Material wiltedMaterial;
    [SerializeField] private Material deadMaterial;


    private PlantStage lastAppliedStage;
    // Start is called before the first frame update
    void Start()
    {
        if(plant == null)
        {
            plant = GetComponent<RainforestPlantEntity>();

        }


        ApplyVisualState();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (plant == null)
        {
            return;
        }
        
        if (plant.plantState.stage != lastAppliedStage)
        {
            ApplyVisualState();
        }
    }

    private void ApplyVisualState()
    {
        var stage = plant.plantState.stage;
        lastAppliedStage = stage;

        switch (stage)
        {
            case PlantStage.Healthy:
                SetActiveModel(useHealthyModel: true);
                healthyRenderer.material = healthyMaterial;
                break;
            case PlantStage.Wilted1:
                SetActiveModel(useHealthyModel: true);
                healthyRenderer.material = wiltedMaterial;
                break;
            case PlantStage.Wilted2:
                SetActiveModel(useHealthyModel: false);
                wiltedRenderer.material = wiltedMaterial;
                break;
            case PlantStage.Dead:
                SetActiveModel(useHealthyModel: false);
                wiltedRenderer.material = deadMaterial;
                break;


        }
    }

    private void SetActiveModel(bool useHealthyModel)
    {
        if (healthyModel != null)
        {
            healthyModel.SetActive(useHealthyModel);
        }
        if(wiltedModel != null)
        {
            wiltedModel.SetActive(!useHealthyModel);
        }
    }
}
