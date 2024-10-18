using UnityEngine;
using TMPro;

public class GlobalDeathCounter : MonoBehaviour
{
    private static int globalDeathCount = 0; // Statyczna zmienna przechowuj�ca globaln� liczb� zgon�w
    private TextMeshProUGUI globalDeathCountText; // TextMeshProUGUI komponent do wy�wietlania liczby zgon�w
    private static bool playerJustDied = false; // Flaga, aby unikn�� wielokrotnego naliczania tego samego zgonu

    private void Awake()
    {
        LoadGlobalDeathCount(); // �adowanie globalnej liczby zgon�w przy starcie

        // Pobierz komponent TextMeshProUGUI
        globalDeathCountText = GetComponent<TextMeshProUGUI>();
        if (globalDeathCountText == null)
        {
            Debug.LogError("TextMeshProUGUI nie zosta� znaleziony na obiekcie GlobalDeathCounter!");
        }
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
        // Zwi�ksz licznik zgon�w tylko, je�li gracz faktycznie zgin�� i naliczono tylko raz
        if (!playerJustDied)
        {
            globalDeathCount++; // Zwi�ksz liczb� zgon�w globalnych
            playerJustDied = true; // Ustaw flag�, �e gracz w�a�nie zgin��
            UpdateGlobalDeathCountText(); // Zaktualizuj tekst po zwi�kszeniu
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
        UpdateGlobalDeathCountText(); // Zaktualizuj tekst na pocz�tku
    }

    // Ta funkcja musi by� wywo�ana, gdy gracz si� odrodzi
    public static void ResetDeathFlag()
    {
        playerJustDied = false; // Resetujemy flag� przy odrodzeniu gracza, aby mog�y by� naliczane kolejne zgony
    }

    private void OnApplicationQuit()
    {
        SaveGlobalDeathCount(); // Zapisz globalne zgony przy wy��czaniu aplikacji
    }
}
