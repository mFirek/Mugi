using UnityEngine;
using TMPro;

public class DeathCountText : MonoBehaviour
{
    private int deathCount = 0;
    private TextMeshProUGUI deathCountText;

    private void Awake()
    {
        // Znajd� komponent TextMeshProUGUI
        deathCountText = GetComponent<TextMeshProUGUI>();

        if (deathCountText == null)
        {
            Debug.LogError("TextMeshProUGUI nie zosta� znaleziony na obiekcie!");
        }
    }

    private void Start()
    {
        // Za�aduj globaln� liczb� zgon�w
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
        deathCount++;  // Zwi�ksz lokaln� liczb� zgon�w
        GlobalDeathCounter.IncrementGlobalDeathCount(); // Zwi�ksz globaln� liczb� zgon�w
        Debug.Log("Gracz zgin��. Liczba zgon�w: " + deathCount);
        UpdateDeathCountText();
    }

    private void UpdateDeathCountText()
    {
        deathCountText.text = "Deaths: " + deathCount;
    }

    private void LoadDeathCount()
    {
        // �adowanie lokalnej liczby zgon�w
        deathCount = PlayerPrefs.GetInt("LocalDeathCount", 0); // Domy�lnie 0
        UpdateDeathCountText(); // Uaktualnij tekst
    }

    public void SaveLocalDeathCount()
    {
        PlayerPrefs.SetInt("LocalDeathCount", deathCount);
    }
}
