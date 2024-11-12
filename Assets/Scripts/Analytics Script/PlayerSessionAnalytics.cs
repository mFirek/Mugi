using UnityEngine;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class PlayerSessionAnalytics : MonoBehaviour
{
    // Statyczna metoda, któr¹ mo¿na wywo³aæ bez tworzenia instancji klasy
    public static void SendSessionEndEvent()
    {
        // Pobierz ca³kowit¹ liczbê zgonów z PlayerPrefs
        int totalDeaths = PlayerPrefs.GetInt("GlobalDeathCount", 0);

        // Przygotowanie danych do wys³ania
        Dictionary<string, object> sessionData = new Dictionary<string, object>()
        {
            { "total_deaths", totalDeaths }
        };

        // Wysy³anie zdarzenia analitycznego
        AnalyticsResult result = Analytics.CustomEvent("session_end", sessionData);
        Debug.Log("Zdarzenie sesji wys³ane: Liczba zgonów = " + totalDeaths);
    }
}
