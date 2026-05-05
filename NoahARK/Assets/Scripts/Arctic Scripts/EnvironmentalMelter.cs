using UnityEngine;

public class EnvironmentalMelter : MonoBehaviour
{
    public TemperatureFault tempSystem;
    public float slightMeltTemp = 32f;
    public float criticalMeltTemp = 50f;

    [Header("Model Data")]
    public Mesh healthyMesh;
    public Mesh meltyMesh;
    public Mesh veryMeltyMesh;

    [Header("Material Data (Optional)")]
    public Material healthyMat;
    public Material meltyMat;
    public Material veryMeltyMat;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private int currentState = -1;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (tempSystem == null) return;

        int targetState = 0;
        if (tempSystem.currentTemp >= criticalMeltTemp) targetState = 2;
        else if (tempSystem.currentTemp >= slightMeltTemp) targetState = 1;

        if (targetState != currentState) SwapModel(targetState);
    }

    void SwapModel(int state)
    {
        currentState = state;
        if (state == 0) Apply(healthyMesh, healthyMat);
        else if (state == 1) Apply(meltyMesh, meltyMat);
        else if (state == 2) Apply(veryMeltyMesh, veryMeltyMat);
    }

    void Apply(Mesh m, Material mat)
    {
        if (m != null) meshFilter.mesh = m;
        if (mat != null) meshRenderer.material = mat;
    }
}