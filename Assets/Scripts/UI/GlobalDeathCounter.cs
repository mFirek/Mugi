using UnityEngine;
using TMPro;

public class GlobalDeathCounter : MonoBehaviour
{
    private static int globalDeathCount = 0; // Statyczna zmienna do przechowywania globalnych zgon�w
    private TextMeshProUGUI globalDeathCountText; // Komponent TextMeshProUGUI do wy�wietlania liczby zgon�w

    private void Awake()
    {
        LoadGlobalDeathCount(); // �adowanie globalnych zgon�w podczas uruchamiania

        // Pobierz komponent TextMeshProUGUI
        globalDeathCountText = GetComponent<TextMeshProUGUI>();
        if (globalDeathCountText == null)
        {
            Debug.LogError("TextMeshProUGUI nie zosta� znaleziony na obiekcie GlobalDeathCounter!");
        }
    }

    public static void SaveGlobalDeathCount()
    {
        PlayerPrefs.SetInt("GlobalDeathCount", globalDeathCount); // Zapisz globaln� liczb� zgon�w
        PlayerPrefs.Save(); // Upewnij si�, �e zmiany s� zapisane
    }

    public void LoadGlobalDeathCount()
    {
        globalDeathCount = PlayerPrefs.GetInt("GlobalDeathCount", 0); // �aduj globaln� liczb� zgon�w
    }

    public static void IncrementGlobalDeathCount()
    {
        globalDeathCount++; // Inkrementacja globalnych zgon�w
        UpdateGlobalDeathCountText(); // Aktualizacja tekstu po inkrementacji
    }

    public static void UpdateGlobalDeathCountText()
    {
        // Znajd� wszystkie obiekty typu GlobalDeathCounter w scenie
        GlobalDeathCounter[] counters = FindObjectsOfType<GlobalDeathCounter>();
        foreach (var counter in counters)
        {
            if (counter.globalDeathCountText != null)
            {
                counter.globalDeathCountText.text = "Global Deaths: " + globalDeathCount; // Ustaw tekst
            }
        }
    }

    private void Start()
    {
        UpdateGlobalDeathCountText(); // Zaktualizuj tekst na pocz�tku
    }

    private void OnApplicationQuit()
    {
        SaveGlobalDeathCount(); // Zapisz globalne zgony przy zamykaniu aplikacji
    }
}
