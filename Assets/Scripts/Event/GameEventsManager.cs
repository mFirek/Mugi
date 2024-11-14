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
        GlobalDeathCounter.IncrementGlobalDeathCount();
    }

    // Ustawienie danych o œmierci gracza
    public static void SetDeathData(string level, Vector3 position, string cause)
    {
        currentLevel = level;
        deathPosition = position;
        causeOfDeath = cause;
    }

    private void RespawnPlayer()
    {
        GlobalDeathCounter.ResetDeathFlag(); // Reset flagi
    }
}
