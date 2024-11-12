using UnityEngine;
using TMPro;

public class GlobalDeathCounter : MonoBehaviour
{
    private static int globalDeathCount = 0; // Statyczna zmienna przechowuj¹ca globaln¹ liczbê zgonów
    private TextMeshProUGUI globalDeathCountText; // Komponent TextMeshProUGUI do wyœwietlania liczby zgonów
    private static bool isDeathIncremented = false; // Flaga do ochrony przed podwójnym naliczaniem przy pierwszym zgonie

    private void Awake()
    {
        LoadGlobalDeathCount(); // £adowanie globalnej liczby zgonów przy starcie

        // Pobierz komponent TextMeshProUGUI
        globalDeathCountText = GetComponent<TextMeshProUGUI>();
        if (globalDeathCountText == null)
        {
            Debug.LogError("TextMeshProUGUI nie zosta³ znaleziony na obiekcie GlobalDeathCounter!");
        }

        // Resetuj flagê na pocz¹tku
        isDeathIncremented = false; // Flaga do zabezpieczenia przed podwójnym naliczaniem
    }

    // Zapisz liczbê globalnych zgonów do PlayerPrefs
    public static void SaveGlobalDeathCount()
    {
        PlayerPrefs.SetInt("GlobalDeathCount", globalDeathCount); // Zapisz liczbê globalnych zgonów
        PlayerPrefs.Save(); // Zapisz zmiany
    }

    // Wczytaj liczbê globalnych zgonów z PlayerPrefs
    public void LoadGlobalDeathCount()
    {
        globalDeathCount = PlayerPrefs.GetInt("GlobalDeathCount", 0); // Za³aduj liczbê globalnych zgonów
    }

    // Zwiêkszenie globalnej liczby zgonów
    public static void IncrementGlobalDeathCount()
    {
        // SprawdŸ, czy liczba zgonów ju¿ nie zosta³a zwiêkszona
        if (!isDeathIncremented)
        {
            globalDeathCount++; // Zwiêksz liczbê zgonów globalnych
            isDeathIncremented = true; // Zapobiega podwójnemu naliczaniu
            SaveGlobalDeathCount(); // Zapisz now¹ wartoœæ globalnej liczby zgonów w PlayerPrefs
            UpdateGlobalDeathCountText(); // Zaktualizuj tekst po zwiêkszeniu
        }
    }

    // Aktualizacja tekstu wyœwietlanego na ekranie
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

    // Resetowanie flagi po odrodzeniu gracza
    public static void ResetDeathFlag()
    {
        isDeathIncremented = false; // Resetujemy dodatkow¹ flagê zabezpieczaj¹c¹
    }

    private void Start()
    {
        // Zaktualizuj tekst na pocz¹tku
        UpdateGlobalDeathCountText();
    }

    private void OnApplicationQuit()
    {
        SaveGlobalDeathCount(); // Zapisz globaln¹ liczbê zgonów przy wy³¹czaniu aplikacji
    }
}
