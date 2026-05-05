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

    [Header("Stats Controllers")]
    [SerializeField] public WakeCycleManager wakeCycleManager;
    [SerializeField] public RainforestBiomeController rainforestBiomeController;
    [SerializeField] public AquaticBiomeController aquaticBiomeController;
    [SerializeField] public ArcticBiomeController arcticBiomeController;

    [Header("Player Settings")]
    [Tooltip("Drag your object with the PlayerMovement script here")]
    [SerializeField] private MonoBehaviour locomotionScript;


    private void Awake()
    {
        fadePanel.alpha = 0f;
        statsPanel.SetActive(false);
    }

    // --- NEW: THE GAME OVER CHECKER ---
    private bool AreAllBiomesExtinct()
    {
        // Added .ToString() to the end of health so it can be compared to text!
        bool rainforestDead = rainforestBiomeController.State.health.ToString() == "Extinct";
        bool aquaticDead = aquaticBiomeController.State.health.ToString() == "Extinct";

        // Arctic is already comparing enum-to-enum, so it stays the same
        bool arcticDead = arcticBiomeController.currentHealth == ArcticBiomeController.ArcticHealth.Extinct;

        return rainforestDead && aquaticDead && arcticDead;
    }
    // -----------------------------------

    public IEnumerator ShowStats()
    {
        // 1. DISABLE MOVEMENT & PHYSICS
        if (locomotionScript != null) locomotionScript.enabled = false;
        StopPlayerPhysics();

        // 2. CHECK FOR GAME OVER FIRST
        if (AreAllBiomesExtinct())
        {
            yield return ShowGameOverScreen();
            yield break; // Stops the rest of the normal stats from playing
        }

        // 3. NORMAL STATS (If they haven't lost yet)
        var rainforestState = rainforestBiomeController.State;
        var aquaticState = aquaticBiomeController.State;
        var arcticState = arcticBiomeController.State;
        var worldState = wakeCycleManager.State;

        statsText.color = Color.white; // Ensure text is white for normal stats
        statsText.text =
            "Year " + worldState.currentYear + " complete.\n\n" +
            "Biome Health Summary:\n" +
            "Rainforest Biome: " + rainforestState.health + "\n" +
            "Coral Reef Biome: " + aquaticState.health + "\n" +
            "Arctic Biome: " + arcticState.health;

        statsPanel.SetActive(true);
        yield return new WaitForSeconds(cutSceneDuration);
        statsPanel.SetActive(false);

        // RE-ENABLE MOVEMENT
        if (locomotionScript != null) locomotionScript.enabled = true;
    }

    public IEnumerator ShowEnding()
    {
        if (locomotionScript != null) locomotionScript.enabled = false;
        StopPlayerPhysics();

        // CHECK FOR GAME OVER HERE TOO (Just in case they lose on the very last year)
        if (AreAllBiomesExtinct())
        {
            yield return ShowGameOverScreen();
            yield break;
        }

        var rainforestState = rainforestBiomeController.State;
        var aquaticState = aquaticBiomeController.State;
        var arcticState = arcticBiomeController.State;
        var worldState = wakeCycleManager.State;

        statsText.color = Color.white;
        statsText.text =
            "Year " + worldState.currentYear + " complete.\n\n" +
            "Biome Health Summary:\n" +
            "Rainforest Biome: " + rainforestState.health + "\n" +
            "Coral Reef Biome: " + aquaticState.health + "\n" +
            "Arctic Biome: " + arcticState.health;

        statsPanel.SetActive(true);
        yield return new WaitForSeconds(3f);

        statsText.text = "Counting is impossible, but we estimate there are at least 100 billion stars in our own galaxy\r\nAnd for every star, we can generally expect at least one planet.\r\nThat is, at minimum, 100,000,000,000 planets within our galactic neighborhood,\r\nAnd one known to have life.\r\nYet we, as humans, have only just begun to explore the galaxy around us,\r\nAnd we can’t yet rule out life on other planets in our own solar system.\r\nLife could be right around the galactic corner, or could be entirely unique.\r\nBut you are unique.\r\nOut of 8,300,000,000 people, you are the only you that has ever existed or will ever exist.\r\nOut of 100,000,000,000 planets in our galaxy, this is the one you experience.\r\nAn amalgamation of particles that have had a life that will never be lived by another.\r\nThat is worth protecting.\r\nThat is worth living.\r\n";

        yield break;
    }

    // --- NEW: THE GAME OVER SCREEN ---
    private IEnumerator ShowGameOverScreen()
    {
        statsPanel.SetActive(true);

        // Make the text dramatic
        statsText.color = Color.red;
        statsText.text =
            "ALL BIOMES EXTINCT.\n\n" +
            "ECOLOGICAL COLLAPSE DETECTED.\n\n" +
            "YOU LOSE.";

        // FREEZE THE GAME
        // Setting Time.timeScale to 0 stops all Update() loops, physics, and timers in Unity.
        Time.timeScale = 0f;

        // We do NOT turn the panel off or re-enable movement. 
        // They are permanently stuck looking at their failure until they restart the game!
        yield break;
    }

    private void StopPlayerPhysics()
    {
        if (locomotionScript == null) return;

        Rigidbody rb = locomotionScript.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public IEnumerator FadeOut(float duration)
    {
        yield return DoFade(0f, 1f, duration);
    }

    public IEnumerator FadeIn(float duration)
    {
        yield return DoFade(1f, 0f, duration);
    }

    private IEnumerator DoFade(float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            fadePanel.alpha = Mathf.Lerp(from, to, t / duration);
            t += Time.deltaTime; // Note: Time.deltaTime becomes 0 if Time.timeScale is 0!
            yield return null;
        }
        fadePanel.alpha = to;
    }
}