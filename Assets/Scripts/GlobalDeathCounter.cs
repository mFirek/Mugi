using UnityEngine;
using TMPro;

public class GlobalDeathCounter : MonoBehaviour
{
    private static int globalDeathCount = 0; // Static variable to store global deaths
    private TextMeshProUGUI globalDeathCountText; // TextMeshProUGUI component to display number of deaths

    private void Awake()
    {
        LoadGlobalDeathCount(); // Loading global deaths at startup

        // Download TextMeshProUGUI component
        globalDeathCountText = GetComponent<TextMeshProUGUI>();
        if (globalDeathCountText == null)
        {
            Debug.LogError("TextMeshProUGUI nie zosta³ znaleziony na obiekcie GlobalDeathCounter!");
        }
    }

    public static void SaveGlobalDeathCount()
    {
        PlayerPrefs.SetInt("GlobalDeathCount", globalDeathCount); // Record the global number of deaths
        PlayerPrefs.Save(); // Make sure the changes are saved
    }

    public void LoadGlobalDeathCount()
    {
        globalDeathCount = PlayerPrefs.GetInt("GlobalDeathCount", 0); // Load global death count
    }

    public static void IncrementGlobalDeathCount()
    {
        globalDeathCount++; // Incremental global deaths
        UpdateGlobalDeathCountText(); // Update text after incrementation
    }

    public static void UpdateGlobalDeathCountText()
    {
        // Find all objects of type GlobalDeathCounter in the scene
        GlobalDeathCounter[] counters = FindObjectsOfType<GlobalDeathCounter>();
        foreach (var counter in counters)
        {
            if (counter.globalDeathCountText != null)
            {
                counter.globalDeathCountText.text = "Global Deaths: " + globalDeathCount; // Set text
            }
        }
    }

    private void Start()
    {
        UpdateGlobalDeathCountText(); // Update the text at the beginning
    }

    private void OnApplicationQuit()
    {
        SaveGlobalDeathCount(); // Save global deaths on application shutdown
    }
}