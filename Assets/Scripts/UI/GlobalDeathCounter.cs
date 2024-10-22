using UnityEngine;
using TMPro;

public class GlobalDeathCounter : MonoBehaviour
{
    private static int globalDeathCount = 0; // Statyczna zmienna przechowuj�ca globaln� liczb� zgon�w
    private TextMeshProUGUI globalDeathCountText; // TextMeshProUGUI komponent do wy�wietlania liczby zgon�w
    private static bool playerJustDied = false; // Flaga, aby unikn�� wielokrotnego naliczania tego samego zgonu
    private static bool isDeathIncremented = false; // Nowa flaga do ochrony przed podw�jnym naliczaniem przy pierwszym zgonie

    private void Awake()
    {
        LoadGlobalDeathCount(); // �adowanie globalnej liczby zgon�w przy starcie

        // Pobierz komponent TextMeshProUGUI
        globalDeathCountText = GetComponent<TextMeshProUGUI>();
        if (globalDeathCountText == null)
        {
            Debug.LogError("TextMeshProUGUI nie zosta� znaleziony na obiekcie GlobalDeathCounter!");
        }

        // Resetuj flagi na pocz�tku
        playerJustDied = false;
        isDeathIncremented = false; // Flaga do zabezpieczenia przed podw�jnym naliczaniem
    }

    public static void SaveGlobalDeathCount()
    {
        PlayerPrefs.SetInt("GlobalDeathCount", globalDeathCount); // Zapisz liczb� zgon�w globalnych
        PlayerPrefs.Save(); // Zapisz zmiany
    }

    public void LoadGlobalDeathCount()
    {
        globalDeathCount = PlayerPrefs.GetInt("GlobalDeathCount", 0); // Za�aduj liczb� globalnych zgon�w
    }

    public static void IncrementGlobalDeathCount()
    {
        Debug.Log("Wywo�ano IncrementGlobalDeathCount"); // Debugowanie wywo�ania funkcji

        // Sprawd�, czy liczba zgon�w ju� nie zosta�a zwi�kszona
        if (!playerJustDied && !isDeathIncremented)
        {
            Debug.Log("Gracz w�a�nie zgin�� po raz pierwszy, zwi�kszamy licznik zgon�w.");
            globalDeathCount++; // Zwi�ksz liczb� zgon�w globalnych
            playerJustDied = true; // Ustaw flag�, �e gracz w�a�nie zgin��
            isDeathIncremented = true; // Zapobiega podw�jnemu naliczaniu
            UpdateGlobalDeathCountText(); // Zaktualizuj tekst po zwi�kszeniu
        }
        else
        {
            Debug.Log("Gracz ju� wcze�niej zgin��, nie naliczamy ponownie.");
        }
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
        // Zaktualizuj tekst na pocz�tku
        UpdateGlobalDeathCountText();
    }

    // Ta funkcja musi by� wywo�ana, gdy gracz si� odrodzi
    public static void ResetDeathFlag()
    {
        Debug.Log("Resetowanie flagi zgonu. Gracz si� odrodzi�.");
        playerJustDied = false; // Resetujemy flag� przy odrodzeniu gracza, aby mog�y by� naliczane kolejne zgony
        isDeathIncremented = false; // Resetujemy dodatkow� flag� do zabezpieczenia
    }

    private void OnApplicationQuit()
    {
        SaveGlobalDeathCount(); // Zapisz globalne zgony przy wy��czaniu aplikacji
    }
}
