using UnityEngine;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class PlayerSessionAnalytics : MonoBehaviour
{
    public static int totalDeaths = 0;

    // Wywo³aj tê metodê w momencie œmierci gracza
    public void OnPlayerDeath()
    {
        totalDeaths++;
    }

    // Wywo³aj na koniec sesji gry
    public void SendSessionEndEvent()
    {
        Dictionary<string, object> sessionData = new Dictionary<string, object>()
        {
            { "total_deaths", totalDeaths }
        };

        AnalyticsResult result = Analytics.CustomEvent("session_end", sessionData);
        Debug.Log("Liczba zgonów podczas sesji: " + totalDeaths);

        totalDeaths = 0; // Resetowanie licznika na kolejn¹ sesjê
    }
}
