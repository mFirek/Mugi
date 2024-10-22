using UnityEngine;
using TMPro;

public class GlobalDeathCounter : MonoBehaviour
{
    private static int globalDeathCount = 0; // Statyczna zmienna przechowuj¹ca globaln¹ liczbê zgonów
    private TextMeshProUGUI globalDeathCountText; // TextMeshProUGUI komponent do wyœwietlania liczby zgonów
    private static bool playerJustDied = false; // Flaga, aby unikn¹æ wielokrotnego naliczania tego samego zgonu
    private static bool isDeathIncremented = false; // Nowa flaga do ochrony przed podwójnym naliczaniem przy pierwszym zgonie

    private void Awake()
    {
        LoadGlobalDeathCount(); // £adowanie globalnej liczby zgonów przy starcie

        // Pobierz komponent TextMeshProUGUI
        globalDeathCountText = GetComponent<TextMeshProUGUI>();
        if (globalDeathCountText == null)
        {
            Debug.LogError("TextMeshProUGUI nie zosta³ znaleziony na obiekcie GlobalDeathCounter!");
        }

        // Resetuj flagi na pocz¹tku
        playerJustDied = false;
        isDeathIncremented = false; // Flaga do zabezpieczenia przed podwójnym naliczaniem
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
        Debug.Log("Wywo³ano IncrementGlobalDeathCount"); // Debugowanie wywo³ania funkcji

        // SprawdŸ, czy liczba zgonów ju¿ nie zosta³a zwiêkszona
        if (!playerJustDied && !isDeathIncremented)
        {
            Debug.Log("Gracz w³aœnie zgin¹³ po raz pierwszy, zwiêkszamy licznik zgonów.");
            globalDeathCount++; // Zwiêksz liczbê zgonów globalnych
            playerJustDied = true; // Ustaw flagê, ¿e gracz w³aœnie zgin¹³
            isDeathIncremented = true; // Zapobiega podwójnemu naliczaniu
            UpdateGlobalDeathCountText(); // Zaktualizuj tekst po zwiêkszeniu
        }
        else
        {
            Debug.Log("Gracz ju¿ wczeœniej zgin¹³, nie naliczamy ponownie.");
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
        // Zaktualizuj tekst na pocz¹tku
        UpdateGlobalDeathCountText();
    }

    // Ta funkcja musi byæ wywo³ana, gdy gracz siê odrodzi
    public static void ResetDeathFlag()
    {
        Debug.Log("Resetowanie flagi zgonu. Gracz siê odrodzi³.");
        playerJustDied = false; // Resetujemy flagê przy odrodzeniu gracza, aby mog³y byæ naliczane kolejne zgony
        isDeathIncremented = false; // Resetujemy dodatkow¹ flagê do zabezpieczenia
    }

    private void OnApplicationQuit()
    {
        SaveGlobalDeathCount(); // Zapisz globalne zgony przy wy³¹czaniu aplikacji
    }
}
