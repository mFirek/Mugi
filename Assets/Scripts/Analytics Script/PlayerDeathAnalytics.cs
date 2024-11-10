using UnityEngine;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class PlayerDeathAnalytics : MonoBehaviour
{
    // Wywo³aj tê metodê w momencie œmierci gracza
    public void SendPlayerDeathEvent(int level, Vector3 deathPosition, string causeOfDeath)
    {
        // Przyk³ad danych o œmierci gracza, które chcesz przes³aæ
        Dictionary<string, object> deathData = new Dictionary<string, object>()
        {
            { "level", level }, // Poziom, na którym zgin¹³ gracz
            { "position_x", deathPosition.x }, // Pozycja X œmierci
            { "position_y", deathPosition.y }, // Pozycja Y œmierci
            { "position_z", deathPosition.z }, // Pozycja Z œmierci
            { "cause_of_death", causeOfDeath } // Przyczyna œmierci (np. "enemy", "fall", "trap")
        };

        // Wys³anie Custom Event do Unity Analytics
        AnalyticsResult result = Analytics.CustomEvent("player_death", deathData);

        if (result == AnalyticsResult.Ok)
        {
            Debug.Log("Wydarzenie 'player_death' zosta³o pomyœlnie wys³ane.");
        }
        else
        {
            Debug.LogWarning("Wys³anie wydarzenia 'player_death' nie powiod³o siê: " + result);
        }
    }
}
