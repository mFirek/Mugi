using UnityEngine;
using UnityEngine.Analytics;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerDeathAnalytics : MonoBehaviour
{
    private bool isDead = false;  // Flaga informuj¹ca, czy gracz zgin¹³

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // SprawdŸ, czy obiekt, z którym gracz zderzy³ siê, jest odpowiedni¹ przeszkod¹
            if (collision.gameObject.CompareTag("Spike") || collision.gameObject.CompareTag("Lava"))
            {
                // Pobierz aktualn¹ nazwê poziomu
                string level = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

                // Pobierz pozycjê gracza w momencie œmierci
                Vector3 deathPosition = transform.position;

                // Pobierz nazwê obiektu, z którym gracz zderzy³ siê
                string causeOfDeath = collision.gameObject.name;

                // Ustaw dane œmierci w GameEventsManager
                GameEventsManager.SetDeathData(level, deathPosition, causeOfDeath);

                // Wywo³aj zdarzenie œmierci
                if (GameEventsManager.instance != null)
                {
                    GameEventsManager.instance.PlayerDied();
                }
                else
                {
                    Debug.LogError("Nie znaleziono instancji GameEventsManager!");
                }
            }
        }
    }

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
            { "cause_of_death", causeOfDeath } // Przyczyna œmierci (np. "enemy", "trap")
        };

        // Wys³anie Custom Event do Unity Analytics
        AnalyticsResult result = Analytics.CustomEvent("player_death", deathData);

        // Sprawdzamy wynik wysy³ania zdarzenia
        if (result == AnalyticsResult.Ok)
        {
            Debug.Log("Wydarzenie 'player_death' zosta³o pomyœlnie wys³ane.");
        }
        else
        {
            Debug.LogWarning("Wys³anie wydarzenia 'player_death' nie powiod³o siê: " + result);
        }

        // Zalogowanie szczegó³ów
        Debug.Log($"Zdarzenie œmierci: Level: {level}, Position: {deathPosition}, Cause of Death: {causeOfDeath}");
    }

}
