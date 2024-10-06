using UnityEngine;
using TMPro;

public class DeathCountText : MonoBehaviour
{
    private int deathCount = 0;
    private TextMeshProUGUI deathCountText;

    private void Awake()
    {
        // ZnajdŸ komponent TextMeshProUGUI
        deathCountText = GetComponent<TextMeshProUGUI>();

        if (deathCountText == null)
        {
            Debug.LogError("TextMeshProUGUI nie zosta³ znaleziony na obiekcie!");
        }
    }

    private void Start()
    {
        // Za³aduj globaln¹ liczbê zgonów
        LoadDeathCount();

        // Zarejestruj zdarzenie zgonu
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
        deathCount++;  // Zwiêksz lokaln¹ liczbê zgonów
        GlobalDeathCounter.IncrementGlobalDeathCount(); // Zwiêksz globaln¹ liczbê zgonów
        Debug.Log("Gracz zgin¹³. Liczba zgonów: " + deathCount);
        UpdateDeathCountText();
    }

    private void UpdateDeathCountText()
    {
        deathCountText.text = "Deaths: " + deathCount;
    }

    private void LoadDeathCount()
    {
        // £adowanie lokalnej liczby zgonów
        deathCount = PlayerPrefs.GetInt("LocalDeathCount", 0); // Domyœlnie 0
        UpdateDeathCountText(); // Uaktualnij tekst
    }

    public void SaveLocalDeathCount()
    {
        PlayerPrefs.SetInt("LocalDeathCount", deathCount);
    }
}
