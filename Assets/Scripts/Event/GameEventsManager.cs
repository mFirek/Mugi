using System;  // Dodane, aby mieæ dostêp do Action
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance;

    public event Action onPlayerDeath;  // U¿ywamy Action dla zdarzeñ bez parametrów

    // Statyczne w³aœciwoœci przechowuj¹ce dane o œmierci gracza
    public static Vector3 deathPosition;
    public static string causeOfDeath;
    public static string currentLevel;
    private static string deathtag;

    private void Awake()
    {
        // Sprawdzenie, czy istnieje ju¿ instancja GameEventsManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Zapobiegamy niszczeniu obiektu podczas zmiany sceny
        }
        else
        {
            Destroy(gameObject);  // Usuwamy dodatkowy obiekt, jeœli ju¿ istnieje instancja
        }
    }

    // Metoda wywo³uj¹ca zdarzenie œmierci gracza
    public void PlayerDied()
    {
        // Wywo³anie zdarzenia œmierci
        onPlayerDeath?.Invoke();
        GlobalDeathCounter.ResetDeathFlag();  // Resetuj flagê

        // Jeœli s¹ zarejestrowane dane o œmierci gracza, wypisz je w logu
        Debug.Log($"Zdarzenie œmierci: Poziom: {currentLevel}, Pozycja: {deathPosition}, Przyczyna œmierci: {causeOfDeath}");

        // Wywo³anie analityki w momencie œmierci gracza
        SendDeathEventToAnalytics();

        GlobalDeathCounter.IncrementGlobalDeathCount();
    }

    // Ustawienie danych o œmierci gracza
    public static void SetDeathData(string level, Vector3 position, string cause, string causeOfDeathTag)
    {
        currentLevel = level;
        deathPosition = position;
        causeOfDeath = cause;
        deathtag = causeOfDeathTag; // Przypisanie tagu przyczyny œmierci
    }


    private void RespawnPlayer()
    {
        GlobalDeathCounter.ResetDeathFlag(); // Reset flagi
    }

    // Nowa metoda wysy³aj¹ca dane o œmierci do analityki
    private void SendDeathEventToAnalytics()
    {
        // SprawdŸ, czy jest dostêpny skrypt PlayerDeathAnalytics w scenie
        PlayerDeathAnalytics playerDeathAnalytics = FindObjectOfType<PlayerDeathAnalytics>();
        if (playerDeathAnalytics != null)
        {
            // Wywo³anie metody wysy³aj¹cej dane o œmierci do Unity Analytics
            playerDeathAnalytics.SendPlayerDeathEvent(currentLevel, deathPosition, causeOfDeath, deathtag);
            Debug.Log("Dane o œmierci zosta³y wys³ane do Unity Analytics.");
        }
        else
        {
            Debug.LogError("Nie znaleziono skryptu PlayerDeathAnalytics w scenie!");
        }
    }

}
