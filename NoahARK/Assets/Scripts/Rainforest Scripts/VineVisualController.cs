using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineVisualController : MonoBehaviour
{
    [SerializeField] private RainforestPlantEntity plant;

    [SerializeField] private GameObject vineModel;
    [SerializeField] private Renderer vineRenderer;

    [SerializeField] private Material healthyMaterial;
    [SerializeField] private Material wiltedMaterial;
    [SerializeField] private Material deadMaterial;

    private PlantStage lastAppliedStage;

    void Start()
    {
        if (plant == null)
            plant = GetComponent<RainforestPlantEntity>();

        ApplyVisualState();
    }

    void Update()
    {
        if (plant == null) return;

        if (plant.plantState.stage != lastAppliedStage)
            ApplyVisualState();
    }

    private void ApplyVisualState()
    {
        var stage = plant.plantState.stage;
        lastAppliedStage = stage;

        switch (stage)
        {
            case PlantStage.Healthy:
                ApplyMaterial(healthyMaterial);
                break;
            case PlantStage.Wilted1:
                ApplyMaterial(wiltedMaterial);
                break;
            case PlantStage.Wilted2:
                ApplyMaterial(wiltedMaterial);
                break;
            case PlantStage.Dead:
                ApplyMaterial(deadMaterial);
                break;
        }
    }

    private void ApplyMaterial(Material mat)
    {
        if (mat == null) return;

        Renderer[] allRenderers = vineRenderer != null
            ? vineRenderer.GetComponentsInChildren<Renderer>(includeInactive: true)
            : GetComponentsInChildren<Renderer>(includeInactive: true);

        foreach (Renderer r in allRenderers)
        {
            int slotCount = Mathf.Max(r.sharedMaterials.Length, 1);
            Material[] mats = new Material[slotCount];
            for (int i = 0; i < slotCount; i++)
                mats[i] = mat;
            r.sharedMaterials = mats;
        }
    }
}
