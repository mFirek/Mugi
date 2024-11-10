using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlayerBossAnalytics : MonoBehaviour
{
    // Singleton – statyczna instancja klasy
    public static PlayerBossAnalytics Instance { get; private set; }

    private int bossAttempts = 0;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Zachowanie obiektu miêdzy scenami
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        // Subskrybuj zdarzenie œmierci gracza
        if (GameEventsManager.instance != null)
        {
            GameEventsManager.instance.onPlayerDeath += OnPlayerDeath;
        }
    }

    private void OnDisable()
    {
        // Wyrejestruj zdarzenie œmierci gracza
        if (GameEventsManager.instance != null)
        {
            GameEventsManager.instance.onPlayerDeath -= OnPlayerDeath;
        }
    }

    // Zwiêksz licznik prób przy ka¿dym zgonie gracza
    private void OnPlayerDeath()
    {
        bossAttempts++;
        Debug.Log("Podejœcie do walki z bossem: " + bossAttempts);
    }

    // Metoda wysy³aj¹ca zdarzenie po pokonaniu bossa
    public void SendBossDefeatEvent()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;

        // Przygotowanie danych zdarzenia
        Dictionary<string, object> bossDefeatData = new Dictionary<string, object>()
        {
            { "level", currentLevel },
            { "attempts", bossAttempts }
        };

        // Wysy³anie zdarzenia do Unity Analytics
        AnalyticsResult result = Analytics.CustomEvent("boss_defeat", bossDefeatData);
        Debug.Log($"Boss pokonany po {bossAttempts} podejœciach na poziomie {currentLevel}. Wynik Analytics: {result}");

        // Zresetuj licznik po pokonaniu bossa
        bossAttempts = 0;
    }
}
