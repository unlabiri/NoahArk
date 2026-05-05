using UnityEngine;
using UnityEditor;

public class IcicleAutomator : EditorWindow
{
    [MenuItem("Tools/Auto-Fill Icicle Data")]
    public static void SetupIcicles()
    {
        // 1. Find the actual Meshes in your Project files
        // (This assumes your files are named exactly like this in your Assets)
        Mesh melty = FindMesh("MeltyIcicle");
        Mesh veryMelty = FindMesh("VeryMeltyIcicle");

        if (melty == null || veryMelty == null)
        {
            Debug.LogError("Could not find the Melty meshes! Check your file names.");
            return;
        }

        // 2. Find all icicles in the scene
        EnvironmentalMelter[] allMelters = GameObject.FindObjectsOfType<EnvironmentalMelter>();

        foreach (EnvironmentalMelter melter in allMelters)
        {
            // Record for Undo so you don't lose work if it messes up
            Undo.RecordObject(melter, "Auto Fill Icicle");

            // 3. Fill the slots
            melter.meltyMesh = melty;
            melter.veryMeltyMesh = veryMelty;

            // If the healthy mesh is empty, grab it from its current state
            if (melter.healthyMesh == null)
                melter.healthyMesh = melter.GetComponent<MeshFilter>().sharedMesh;

            EditorUtility.SetDirty(melter);
        }

        Debug.Log($"Successfully updated {allMelters.Length} icicles!");
    }

    private static Mesh FindMesh(string name)
    {
        string[] guids = AssetDatabase.FindAssets(name + " t:Mesh");
        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            return AssetDatabase.LoadAssetAtPath<Mesh>(path);
        }
        return null;
    }
}