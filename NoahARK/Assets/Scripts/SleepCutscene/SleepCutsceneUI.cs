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

    public IEnumerator ShowEnding()
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
        yield return new WaitForSeconds(3f);

        statsText.text = "Counting is impossible, but we estimate there are at least 100 billion stars in our own galaxy\r\nAnd for every star, we can generally expect at least one planet.\r\nThat is, at minimum, 100,000,000,000 planets within our galactic neighborhood,\r\nAnd one known to have life.\r\nYet we, as humans, have only just begun to explore the galaxy around us,\r\nAnd we can’t yet rule out life on other planets in our own solar system.\r\nLife could be right around the galactic corner, or could be entirely unique.\r\nBut you are unique.\r\nOut of 8,300,000,000 people, you are the only you that has ever existed or will ever exist.\r\nOut of 100,000,000,000 planets in our galaxy, this is the one you experience.\r\nAn amalgamation of particles that have had a life that will never be lived by another.\r\nThat is worth protecting.\r\nThat is worth living.\r\n";
        yield break;
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