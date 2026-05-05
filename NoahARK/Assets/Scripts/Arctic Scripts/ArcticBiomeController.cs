using UnityEngine;

public class ArcticBiomeController : MonoBehaviour
{
    // By defining these in order, Unity assigns them numbers behind the scenes (0, 1, 2, 3).
    // This makes it super easy to check if a state is "worse" than another!
    public enum ArcticHealth { Healthy, Damaged, Critical, Extinct }

    [Header("Current Status")]
    public ArcticHealth currentState = ArcticHealth.Healthy;

    // A small data wrapper so it matches the exact way your other Biome Controllers talk to the UI
    public ArcticStateData State => new ArcticStateData { health = currentState.ToString() };

    /// <summary>
    /// Call this function from whatever script controls your game's Temperature!
    /// </summary>
    public void CheckTemperature(float currentTemp)
    {
        // If it's already extinct, don't even bother checking the temperature anymore. It's gone.
        if (currentState == ArcticHealth.Extinct) return;

        // Check the highest temperature first.
        if (currentTemp >= 70f)
        {
            currentState = ArcticHealth.Extinct;
            Debug.Log("Arctic Biome is now EXTINCT.");
        }
        // If it's not extinct, check if it should be Critical. 
        // We also check to make sure it isn't ALREADY Critical before downgrading it.
        else if (currentTemp >= 52f && currentState < ArcticHealth.Critical)
        {
            currentState = ArcticHealth.Critical;
            Debug.Log("Arctic Biome is now CRITICAL.");
        }
        // Finally, check for Damaged.
        else if (currentTemp >= 32f && currentState < ArcticHealth.Damaged)
        {
            currentState = ArcticHealth.Damaged;
            Debug.Log("Arctic Biome is now DAMAGED.");
        }
    }
}

// This struct mimics the 'State' object your other controllers use
public struct ArcticStateData
{
    public string health;
}