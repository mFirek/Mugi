using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;
using System.Threading.Tasks; // Dodajemy przestrzeñ nazw dla Task

public class PlayerSessionAnalytics : MonoBehaviour
{
    private static bool isInitialized = false;

    private async void Start()
    {
        // Zainicjalizuj Unity Services tylko raz, podczas startu gry
        await InitializeUnityServices();
    }

    // Statyczna metoda do inicjalizacji Unity Services
    public static async Task InitializeUnityServices()
    {
        try
        {
            await UnityServices.InitializeAsync();
            GiveConsent();
            isInitialized = true;
            Debug.Log("Unity Services zosta³y pomyœlnie zainicjalizowane.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"B³¹d inicjalizacji Unity Services: {e.Message}");
        }
    }

    // Statyczna metoda do udzielania zgody na zbieranie danych
    public static void GiveConsent()
    {
        AnalyticsService.Instance.StartDataCollection();
        Debug.Log("Zgoda na zbieranie danych zosta³a udzielona.");
    }

    // Statyczna metoda do wysy³ania zdarzenia 'session_end'
    public static void SendSessionEndEvent()
    {
        if (!isInitialized)
        {
            Debug.LogError("Unity Services nie zosta³y zainicjalizowane. Nie mo¿na wys³aæ zdarzenia.");
            return;
        }

        // Pobieramy liczbê zgonów z PlayerPrefs
        int totalDeaths = PlayerPrefs.GetInt("GlobalDeathCount", 0);

        // Przygotowanie danych do wys³ania
        Dictionary<string, object> sessionData = new Dictionary<string, object>()
        {
            { "total_deaths", totalDeaths }
        };

        // Wysy³anie zdarzenia do Unity Analytics
        try
        {
            Debug.Log("Próba wys³ania zdarzenia 'session_end'...");
            AnalyticsService.Instance.CustomData("session_end", sessionData);
            AnalyticsService.Instance.Flush();  // Wysy³anie danych natychmiastowo
            Debug.Log($"Zdarzenie 'session_end' zosta³o wys³ane: Liczba zgonów = {totalDeaths}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"B³¹d podczas wysy³ania zdarzenia 'session_end': {e.Message}");
        }
    }
}
