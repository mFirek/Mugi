using UnityEngine;
using TMPro;

public class DeathCountText : MonoBehaviour
{
    private int deathCount = 0;
    private TextMeshProUGUI deathCountText;

    private void Awake()
    {
        // Find the TextMeshProUGUI component
        deathCountText = GetComponent<TextMeshProUGUI>();

        if (deathCountText == null)
        {
            Debug.LogError("TextMeshProUGUI nie zosta³ znaleziony na obiekcie!");
        }
    }

    private void Start()
    {
        // Load the global number of deaths
        LoadDeathCount();

        // Register a death event
        if (GameEventsManager.instance != null)
        {
            GameEventsManager.instance.onPlayerDeath += OnPlayerDeath;
        }

        UpdateDeathCountText();
    }

    private void OnDestroy()
    {
        if (GameEventsManager.instance != null)
        {
            GameEventsManager.instance.onPlayerDeath -= OnPlayerDeath;
        }
    }

    private void OnPlayerDeath()
    {
        deathCount++;  // Increase the local number of deaths
        GlobalDeathCounter.IncrementGlobalDeathCount(); // Increase the global death toll
        Debug.Log("Gracz zgin¹³. Liczba zgonów: " + deathCount);
        UpdateDeathCountText();
    }

    private void UpdateDeathCountText()
    {
        deathCountText.text = "Deaths: " + deathCount;
    }

    private void LoadDeathCount()
    {
        // Loading local death count
        deathCount = PlayerPrefs.GetInt("LocalDeathCount", 0); // Default 0
        UpdateDeathCountText(); // Update text
    }

    public void SaveLocalDeathCount()
    {
        PlayerPrefs.SetInt("LocalDeathCount", deathCount);
    }
}
