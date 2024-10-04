using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathCountText : MonoBehaviour, IDataPresistence
{
    private int deathCount = 0;  
    private TextMeshProUGUI deathCountText;  // TextMeshProUGUI component reference

    private void Awake()
    {
        // Download TextMeshProUGUI component
        deathCountText = GetComponent<TextMeshProUGUI>();

        if (deathCountText == null)
        {
            Debug.LogError("TextMeshProUGUI nie zosta³ znaleziony na obiekcie!");
        }
    }

    // Method to load data
    public void LoadData(GameData gameData)
    {
        this.deathCount = gameData.deathCount;  // Assign the value of deaths from GameData
        Debug.Log("Za³adowano dane: liczba zgonów = " + this.deathCount);
        UpdateDeathCountText();  // Update text
    }

    // Method to record data
    public void SaveData(ref GameData data)
    {
        data.deathCount = this.deathCount;  // Save the value of deaths to GameData
    }

    private void Start()
    {
        // Subscribe to player death event
        if (GameEventsManager.instance != null)
        {
            GameEventsManager.instance.onPlayerDeath += OnPlayerDeath;
            Debug.Log("Zarejestrowano OnPlayerDeath w GameEventsManager.");
        }
        else
        {
            Debug.LogError("Nie znaleziono instancji GameEventsManager!");
        }

        // Initialize text at startup
        UpdateDeathCountText();
    }

    private void OnDestroy()
    {
        // Unsubscribe player death event
        if (GameEventsManager.instance != null)
        {
            GameEventsManager.instance.onPlayerDeath -= OnPlayerDeath;
        }
    }

    private void OnPlayerDeath()

    {
        deathCount++;  // Increase death count
        Debug.Log("Gracz zgin¹³. Liczba zgonów: " + deathCount);  // Log deaths
        UpdateDeathCountText();  // Update text
    }

    private void UpdateDeathCountText()
    {
        // Set text on "Deaths: X"
        deathCountText.text = "Deaths: " + deathCount;
    }
}
