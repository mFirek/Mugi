using UnityEngine;
using TMPro;

public class GlobalDeathCounter : MonoBehaviour
{
    private static int globalDeathCount = 0; // Statyczna zmienna przechowuj¹ca globaln¹ liczbê zgonów
    private TextMeshProUGUI globalDeathCountText; // TextMeshProUGUI komponent do wyœwietlania liczby zgonów
    private static bool playerJustDied = false; // Flaga, aby unikn¹æ wielokrotnego naliczania tego samego zgonu

    private void Awake()
    {
        LoadGlobalDeathCount(); // £adowanie globalnej liczby zgonów przy starcie

        // Pobierz komponent TextMeshProUGUI
        globalDeathCountText = GetComponent<TextMeshProUGUI>();
        if (globalDeathCountText == null)
        {
            Debug.LogError("TextMeshProUGUI nie zosta³ znaleziony na obiekcie GlobalDeathCounter!");
        }
    }

    public static void SaveGlobalDeathCount()
    {
        PlayerPrefs.SetInt("GlobalDeathCount", globalDeathCount); // Zapisz liczbê zgonów globalnych
        PlayerPrefs.Save(); // Zapisz zmiany
    }

    public void LoadGlobalDeathCount()
    {
        globalDeathCount = PlayerPrefs.GetInt("GlobalDeathCount", 0); // Za³aduj liczbê globalnych zgonów
    }

    public static void IncrementGlobalDeathCount()
    {
        // Zwiêksz licznik zgonów tylko, jeœli gracz faktycznie zgin¹³ i naliczono tylko raz
        if (!playerJustDied)
        {
            globalDeathCount++; // Zwiêksz liczbê zgonów globalnych
            playerJustDied = true; // Ustaw flagê, ¿e gracz w³aœnie zgin¹³
            UpdateGlobalDeathCountText(); // Zaktualizuj tekst po zwiêkszeniu
        }
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

    // Ta funkcja musi byæ wywo³ana, gdy gracz siê odrodzi
    public static void ResetDeathFlag()
    {
        playerJustDied = false; // Resetujemy flagê przy odrodzeniu gracza, aby mog³y byæ naliczane kolejne zgony
    }

    private void OnApplicationQuit()
    {
        SaveGlobalDeathCount(); // Zapisz globalne zgony przy wy³¹czaniu aplikacji
    }
}
