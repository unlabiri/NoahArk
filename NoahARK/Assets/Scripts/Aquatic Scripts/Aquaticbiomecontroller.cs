using System.Collections.Generic;
using UnityEngine;

public class AquaticBiomeController : MonoBehaviour
{
    [SerializeField] private AquaticBiomeState state = new AquaticBiomeState();
    public AquaticBiomeState State => state;

    [Tooltip("Drag your 'Corals' parent GameObject here.")]
    [SerializeField] private Transform coralParent;

    [Header("Shared Coral Materials")]
    public Material healthyMaterial;
    public Material bleached1Material;
    public Material bleached2Material;
    public Material deadMaterial;

    // ?? Testing / override ??????????????????????????????????????????????????
    [Header("Debug / Testing")]
    [Tooltip("Force the biome into this health state immediately (play mode only).")]
    public BiomeHealthState forceHealthState;
    [SerializeField] private bool applyForcedState = false;

    private List<AquaticCoralEntity> corals = new();

    // ?? Unity lifecycle ?????????????????????????????????????????????????????
    private void Start()
    {
        if (coralParent != null)
        {
            foreach (var coral in coralParent.GetComponentsInChildren<AquaticCoralEntity>())
                RegisterCoral(coral);
        }

        RefreshAndUpdateState();
    }

    private void Update()
    {
        // Instant override for testing — toggle the bool in Inspector
        if (applyForcedState)
        {
            State.health = forceHealthState;
            applyForcedState = false; // auto-resets so it's a one-shot trigger
            return;
        }

        RefreshAndUpdateState();
    }

    // ?? Public API (called by corals) ???????????????????????????????????????

    /// <summary>
    /// Call this from AquaticCoralEntity.Start() so new corals self-register.
    /// </summary>
    public void RegisterCoral(AquaticCoralEntity coral)
    {
        if (coral == null || corals.Contains(coral)) return;
        corals.Add(coral);
        coral.Initialize(this);
        RefreshAndUpdateState();
    }

    /// <summary>
    /// Corals call this the moment their alive-state changes so the biome
    /// reacts immediately instead of waiting for the next Update tick.
    /// </summary>
    public void OnCoralStateChanged()
    {
        RefreshAndUpdateState();
    }

    // ?? Inspector button (works in Play Mode via context menu) ??????????????
    [ContextMenu("Force Refresh Biome State")]
    public void EditorForceRefresh()
    {
        RefreshAndUpdateState();
        Debug.Log($"[AquaticBiome] Forced refresh ? {State.health} " +
                  $"({State.totalAliveCoralCount}/{State.totalCoralCount} alive)");
    }

    // ?? Internal ????????????????????????????????????????????????????????????
    private void RefreshAndUpdateState()
    {
        PruneDestroyedCorals();
        RefreshCoralCounts();
        UpdateBiomeHealthState();
    }

    /// <summary>Remove slots left behind by destroyed GameObjects.</summary>
    private void PruneDestroyedCorals()
    {
        corals.RemoveAll(c => c == null);
    }

    private void RefreshCoralCounts()
    {
        State.totalCoralCount = corals.Count;
        State.totalAliveCoralCount = 0;
        State.totalDeadCoralCount = 0;

        foreach (var coral in corals)
        {
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

        State.health = alivePercent >= 0.75f ? BiomeHealthState.Healthy
                     : alivePercent >= 0.50f ? BiomeHealthState.Vulnerable
                     : alivePercent > 0f ? BiomeHealthState.Endangered
                                             : BiomeHealthState.Extinct;
    }
}