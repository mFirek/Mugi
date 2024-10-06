using UnityEngine;
using TMPro;

public class GlobalDeathCounter : MonoBehaviour
{
    private static int globalDeathCount = 0; // Statyczna zmienna do przechowywania globalnych zgonów
    private TextMeshProUGUI globalDeathCountText; // Komponent TextMeshProUGUI do wyœwietlania liczby zgonów

    private void Awake()
    {
        LoadGlobalDeathCount(); // £adowanie globalnych zgonów podczas uruchamiania

        // Pobierz komponent TextMeshProUGUI
        globalDeathCountText = GetComponent<TextMeshProUGUI>();
        if (globalDeathCountText == null)
        {
            Debug.LogError("TextMeshProUGUI nie zosta³ znaleziony na obiekcie GlobalDeathCounter!");
        }
    }

    public static void SaveGlobalDeathCount()
    {
        PlayerPrefs.SetInt("GlobalDeathCount", globalDeathCount); // Zapisz globaln¹ liczbê zgonów
        PlayerPrefs.Save(); // Upewnij siê, ¿e zmiany s¹ zapisane
    }

    public void LoadGlobalDeathCount()
    {
        globalDeathCount = PlayerPrefs.GetInt("GlobalDeathCount", 0); // £aduj globaln¹ liczbê zgonów
    }

    public static void IncrementGlobalDeathCount()
    {
        globalDeathCount++; // Inkrementacja globalnych zgonów
        UpdateGlobalDeathCountText(); // Aktualizacja tekstu po inkrementacji
    }

    public static void UpdateGlobalDeathCountText()
    {
        // ZnajdŸ wszystkie obiekty typu GlobalDeathCounter w scenie
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
        UpdateGlobalDeathCountText(); // Zaktualizuj tekst na pocz¹tku
    }

    private void OnApplicationQuit()
    {
        SaveGlobalDeathCount(); // Zapisz globalne zgony przy zamykaniu aplikacji
    }
}
