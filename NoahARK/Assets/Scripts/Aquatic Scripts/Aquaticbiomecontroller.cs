using System.Collections.Generic;
using UnityEngine;

public class AquaticBiomeController : MonoBehaviour
{
    [SerializeField] private AquaticBiomeState state = new AquaticBiomeState();
    public AquaticBiomeState State => state;

    [Tooltip("Drag your 'Corals' parent GameObject here.")]
    [SerializeField] private Transform coralParent;

    [Header("Shared Coral Materials — set once here, all corals use these")]
    public Material healthyMaterial;
    public Material bleached1Material;
    public Material bleached2Material;
    public Material deadMaterial;

    private List<AquaticCoralEntity> corals = new();

    private void Start()
    {
        if (coralParent != null)
        {
            var found = coralParent.GetComponentsInChildren<AquaticCoralEntity>();
            foreach (var coral in found)
            {
                if (coral != null)
                {
                    corals.Add(coral);
                    coral.Initialize(this);
                }
            }
        }

        RefreshCoralCounts();
        UpdateBiomeHealthState();
    }

    public void Update()
    {
        RefreshCoralCounts();
        UpdateBiomeHealthState();
    }

    public void RegisterCoral(AquaticCoralEntity coral)
    {
        if (coral == null || corals.Contains(coral)) return;
        corals.Add(coral);
        RefreshCoralCounts();
        UpdateBiomeHealthState();
    }

    private void RefreshCoralCounts()
    {
        State.totalCoralCount = corals.Count;
        State.totalAliveCoralCount = 0;
        State.totalDeadCoralCount = 0;

        foreach (var coral in corals)
        {
            if (coral == null) continue;
            if (coral.coralState.isAlive)
                State.totalAliveCoralCount++;
            else
                State.totalDeadCoralCount++;
        }
    }

    private void UpdateBiomeHealthState()
    {
        if (State.totalCoralCount <= 0)
        {
            State.health = BiomeHealthState.Extinct;
            return;
        }

        float alivePercent = (float)State.totalAliveCoralCount / State.totalCoralCount;

        if (alivePercent >= 0.75f)
            State.health = BiomeHealthState.Healthy;
        else if (alivePercent >= 0.50f)
            State.health = BiomeHealthState.Vulnerable;
        else if (alivePercent > 0f)
            State.health = BiomeHealthState.Endangered;
        else
            State.health = BiomeHealthState.Extinct;
    }
}