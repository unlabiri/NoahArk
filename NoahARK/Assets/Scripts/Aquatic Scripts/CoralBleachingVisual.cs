using UnityEngine;

public class CoralBleachingVisual : MonoBehaviour
{
    [Header("Renderer")]
    [SerializeField] private Renderer coralRenderer;

    [SerializeField] private string targetMaterialName = "lambert2";

    [Header("Bleaching Colors")]
    [SerializeField] private Color healthyColor = new Color(0.82f, 0.72f, 0.58f);
    [SerializeField] private Color lowBleachColor = new Color(0.88f, 0.82f, 0.72f);
    [SerializeField] private Color mediumBleachColor = new Color(0.92f, 0.88f, 0.82f);
    [SerializeField] private Color highBleachColor = new Color(0.96f, 0.94f, 0.90f);
    [SerializeField] private Color fullyBleachedColor = new Color(1f, 0.98f, 0.95f);

    private Material targetMaterial;

    private void Awake()
    {
        CacheMaterial();
        ApplyHealthy();
    }

    private void CacheMaterial()
    {
        if (coralRenderer == null) return;

        Material[] mats = coralRenderer.materials;

        for (int i = 0; i < mats.Length; i++)
        {
            if (mats[i] != null && mats[i].name.Contains(targetMaterialName))
            {
                targetMaterial = mats[i];
                break;
            }
        }

        if (targetMaterial == null && mats.Length > 0)
        {
            Debug.LogWarning($"{name}: Could not find material '{targetMaterialName}', defaulting to first material.");
            targetMaterial = mats[0];
        }
    }

    public void ApplyHealthy()
    {
        SetColor(healthyColor);
    }

    public void ApplyLowBleaching()
    {
        SetColor(lowBleachColor);
    }

    public void ApplyMediumBleaching()
    {
        SetColor(mediumBleachColor);
    }

    public void ApplyHighBleaching()
    {
        SetColor(highBleachColor);
    }

    public void ApplyFullBleaching()
    {
        SetColor(fullyBleachedColor);
    }

    private void SetColor(Color color)
    {
        if (targetMaterial == null) return;

        if (targetMaterial.HasProperty("_Color"))
            targetMaterial.color = color;

        if (targetMaterial.HasProperty("_BaseColor"))
            targetMaterial.SetColor("_BaseColor", color);
    }
}