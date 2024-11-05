using System;  // Dodane, aby mieæ dostêp do Action
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance;

    public event Action onPlayerDeath;  // U¿ywamy Action dla zdarzeñ bez parametrów

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
        onPlayerDeath?.Invoke();
        GlobalDeathCounter.ResetDeathFlag();
    }

    private void RespawnPlayer()
    {
        GlobalDeathCounter.ResetDeathFlag(); // Reset flagi
    }
}
