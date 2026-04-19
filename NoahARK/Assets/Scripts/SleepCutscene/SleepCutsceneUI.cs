using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SleepCutsceneUI : MonoBehaviour
{
    [Header("Fade")]
    [SerializeField] private CanvasGroup fadePanel;

    [Header("Stats UI")]
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] public float cutSceneDuration = 6f;

    [Header("Stats")]
    [SerializeField] public WakeCycleManager wakeCycleManager;
    [SerializeField] public RainforestBiomeController rainforestBiomeController;
    [SerializeField] public AquaticBiomeController aquaticBiomeController; 

    private void Awake()
    {
        fadePanel.alpha = 0f;
        statsPanel.SetActive(false);
    }

    public IEnumerator FadeOut(float duration)
    {
        yield return DoFade(0f, 1f, duration);
    }

    public IEnumerator FadeIn(float duration)
    {
        yield return DoFade(1f, 0f, duration);
    }

    public IEnumerator ShowStats()
    {
        var rainforestState = rainforestBiomeController.State;
        var aquaticState = aquaticBiomeController.State;
        var worldState = wakeCycleManager.State;

        statsText.text =
            "Year " + worldState.currentYear + " complete.\n\n" +
            "Biome Health Summary:\n" +
            "Rainforest Biome: " + rainforestState.health + "\n" +
            "Coral Reef Biome: " + aquaticState.health + "\n" +
            "Arctic Biome: ";

        statsPanel.SetActive(true);
        yield return new WaitForSeconds(cutSceneDuration);
        statsPanel.SetActive(false);
    }

    private IEnumerator DoFade(float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            fadePanel.alpha = Mathf.Lerp(from, to, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        fadePanel.alpha = to;
    }
}