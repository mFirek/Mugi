using UnityEngine;
using TMPro;

public class DeathCountText : MonoBehaviour
{
    private int deathCount = 0;
    private TextMeshProUGUI deathCountText;

    private void Awake()
    {
        deathCountText = GetComponent<TextMeshProUGUI>();

        if (deathCountText == null)
        {
            Debug.LogError("TextMeshProUGUI nie zosta³ znaleziony na obiekcie!");
        }
    }

    private void Start()
    {
        LoadDeathCount();

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
        deathCount++;
        UpdateDeathCountText();
    }

    private void UpdateDeathCountText()
    {
        deathCountText.text = "Deaths: " + deathCount;
    }

    private void LoadDeathCount()
    {
        deathCount = PlayerPrefs.GetInt("LocalDeathCount", 0);
        UpdateDeathCountText();
    }

    public void SaveLocalDeathCount()
    {
        PlayerPrefs.SetInt("LocalDeathCount", deathCount);
    }

    // Metoda do pobrania liczby zgonów
    public int GetDeathCount()
    {
        return deathCount;
    }
}
